using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    public class CustomScrollRect : ScrollRect
    {
        // [SerializeField] private Vector2Int m_viewCellGrid = new Vector2Int(1, 6);
        private GridLayoutGroup m_gridLayoutGroup;
        private int m_maxCellCount = 0;

        private Vector2Int m_runCellGrid;
        private int m_maxItemCount = 0;
        private int m_viewCount = 0;
        //private Vector2Int m_viewGrid;
        private Vector2 m_itemSize;
        private Vector2 m_offset;//content当前的偏移量
        private float m_deltaX;//x方向补偿
        private float m_deltaY;//y方向补偿
        private int m_maxEndIndex;//最大的索引

        private Vector2 m_limitX;//x方向的极限值
        private Vector2 m_limitY;//y方向的极限值

        private LinkedList<CustomScrollItem> m_itemList = new LinkedList<CustomScrollItem>();


        protected override void SetContentAnchoredPosition(Vector2 position)
        {
            base.SetContentAnchoredPosition(position);
            m_offset = content.anchoredPosition - Vector2.zero;
            SetVertical();
            SetHorizontal();
        }

        public int MaxCellCount
        {
            get
            {
                return m_maxCellCount;
            }
            set
            {
                m_maxCellCount = value;
                CreateViewItem();
            }
        }


        #region 对内接口

        private void CreateViewItem()
        {
            ContentSizeFitter sizeFitter = content.GetComponent<ContentSizeFitter>();
            sizeFitter.horizontalFit = horizontal ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;
            sizeFitter.verticalFit = vertical ? ContentSizeFitter.FitMode.PreferredSize : ContentSizeFitter.FitMode.Unconstrained;

            m_gridLayoutGroup = content.GetComponent<GridLayoutGroup>();
            m_gridLayoutGroup.startAxis = horizontal ? GridLayoutGroup.Axis.Vertical : GridLayoutGroup.Axis.Horizontal;
            // m_runCellGrid = m_viewCellGrid;
            // m_viewCount = m_viewCellGrid.x * m_viewCellGrid.y;
            m_itemSize = m_gridLayoutGroup.cellSize;


            content.anchoredPosition = Vector2.zero;
            ResetViewCell();
            ResetOffset();

            List<CustomScrollItem> itemList = new List<CustomScrollItem>();
            var enumerator = m_itemList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                itemList.Add(enumerator.Current);
            }
            m_itemList.Clear();


            for (int i = m_viewCount; i < itemList.Count; i++)
            {
                if (i > 0)
                    Destroy(itemList[i].gameObject);
                else
                    itemList[i].gameObject.SetActive(false);
            }



            CustomScrollItem item;
            if (itemList.Count == 0)
            {
                item = content.GetChild(0).GetComponent<CustomScrollItem>();
            }
            else
            {
                item = itemList[0];
            }
            m_itemList.AddLast(item);
            item.Index = 0;
            item.gameObject.SetActive(m_maxCellCount > 0);
            int itemCount = itemList.Count;
            for (int i = 1; i < m_viewCount; i++)
            {
                CustomScrollItem childItem;
                if (i >= itemCount)
                {
                    GameObject go = GameObject.Instantiate(item.gameObject);
                    go.transform.SetParent(content);
                    go.transform.localScale = Vector3.one;
                    go.transform.localPosition = Vector3.zero;

                    childItem = go.GetComponent<CustomScrollItem>();
                }
                else
                {
                    childItem = itemList[i];
                }
                childItem.gameObject.SetActive(i < m_maxCellCount);
                childItem.Index = i;
                m_itemList.AddLast(childItem);
            }
        }





        private void ResetOffset()
        {
            RectOffset padding = m_gridLayoutGroup.padding;
            Vector2 spacing = m_gridLayoutGroup.spacing;
            Vector2 viewportSize = new Vector2(viewport.rect.width, viewport.rect.height);
            float contentWidth = m_itemSize.x * m_runCellGrid.x + padding.left + padding.right + spacing.x * (m_runCellGrid.x - 1);
            float contentHeight = m_itemSize.y * m_runCellGrid.y + padding.top + padding.bottom + spacing.y * (m_runCellGrid.y - 1);
            float maxX = -m_itemSize.x * 0.5f - padding.left;
            float minX = -contentWidth + viewportSize.x + m_itemSize.x * 0.5f;
            m_limitX = new Vector2(minX, maxX);
            m_deltaX = m_itemSize.x + spacing.x;

            float minY = m_itemSize.y * 0.5f + padding.top;
            float maxY = contentHeight - viewportSize.y - m_itemSize.y * 0.5f;
            m_limitY = new Vector2(minY, maxY);
            m_deltaY = m_itemSize.y + spacing.y;
        }


        private void ResetGrid()
        {
            GridLayoutGroup.Constraint constraint = m_gridLayoutGroup.constraint;
            m_runCellGrid = Vector2Int.one;
            if (constraint != GridLayoutGroup.Constraint.Flexible)
            {
                m_runCellGrid.x = vertical ? m_gridLayoutGroup.constraintCount : 1;
                m_runCellGrid.y = horizontal ? m_gridLayoutGroup.constraintCount : 1;
            }

            RectOffset padding = m_gridLayoutGroup.padding;
            Vector2 spacing = m_gridLayoutGroup.spacing;
            Vector2 viewportSize = new Vector2(viewport.rect.width, viewport.rect.height);
            if (vertical)
            {
                int row = Mathf.FloorToInt(viewportSize.y / m_itemSize.y) + 2;
                m_runCellGrid.y = row;
            }
            else
            {
                int col = Mathf.FloorToInt(viewportSize.x / m_itemSize.x) + 2;
                m_runCellGrid.x = col;
            }

        }

        private void ResetViewCell()
        {
            ResetGrid();
            m_maxItemCount = m_maxCellCount;
            if (vertical)
            {
                // int rest = m_maxCellCount % m_viewCellGrid.x;
                // int row = m_maxCellCount / m_viewCellGrid.x;
                int rest = m_maxCellCount % m_runCellGrid.x;
                int row = m_maxCellCount / m_runCellGrid.x;
                if (rest != 0)
                {
                    // m_maxItemCount = m_maxCellCount + m_viewCellGrid.x - rest;
                    m_maxItemCount = m_maxCellCount + m_runCellGrid.x - rest;
                }
                if (row < m_runCellGrid.y)
                    m_runCellGrid.y = rest == 0 ? row : row + 1;
            }
            else if (horizontal)
            {
                // int rest = m_maxCellCount % m_viewCellGrid.y;
                // int col = m_maxCellCount / m_viewCellGrid.y;
                int rest = m_maxCellCount % m_runCellGrid.y;
                int col = m_maxCellCount / m_runCellGrid.y;
                if (rest != 0)
                {
                    // m_maxItemCount = m_maxCellCount + m_viewCellGrid.y - rest;
                    m_maxItemCount = m_maxCellCount + m_runCellGrid.y - rest;
                }
                if (col < m_runCellGrid.x)
                    m_runCellGrid.x = rest == 0 ? col : col + 1;
            }
            m_viewCount = m_runCellGrid.x * m_runCellGrid.y;
            m_maxEndIndex = m_maxItemCount - m_viewCount;
        }



        private void SetVertical()
        {
            if (!vertical) return;
            if (m_viewCount >= MaxCellCount) return;
            if (m_offset.y < m_limitY.y && m_offset.y > m_limitY.x) return;
            CustomScrollItem firstItem = m_itemList.First.Value;
            float deltaY = 0;
            if (m_offset.y >= m_limitY.y)
            {
                if (firstItem.Index >= m_maxEndIndex) return;
                for (int i = 0; i < m_runCellGrid.x; i++)
                {
                    CustomScrollItem item = m_itemList.First.Value;
                    m_itemList.RemoveFirst();
                    item.transform.SetAsLastSibling();
                    item.Index += m_viewCount;
                    item.gameObject.SetActive(item.Index < m_maxCellCount);
                    m_itemList.AddLast(item);
                }
                deltaY = m_deltaY;
            }
            else
            {
                if (firstItem.Index <= 0) return;
                for (int i = 0; i < m_runCellGrid.x; i++)
                {
                    CustomScrollItem item = m_itemList.Last.Value;
                    m_itemList.RemoveLast();
                    item.transform.SetAsFirstSibling();
                    item.Index -= m_viewCount;
                    item.gameObject.SetActive(item.Index >= 0);
                    m_itemList.AddFirst(item);
                }
                deltaY = -m_deltaY;
            }

            Vector2 currentPos = content.anchoredPosition;
            currentPos.y -= deltaY;
            content.anchoredPosition = currentPos;
            m_ContentStartPosition.y -= deltaY;
            UpdatePrevData();
        }

        private void SetHorizontal()
        {
            if (!horizontal) return;
            if (m_viewCount >= MaxCellCount) return;
            if (m_offset.x < m_limitX.y && m_offset.x > m_limitX.x) return;
            CustomScrollItem firstItem = m_itemList.First.Value;
            float deltaX = 0;
            if (m_offset.x >= m_limitX.y)
            {
                if (firstItem.Index <= 0) return;
                for (int i = 0; i < m_runCellGrid.y; i++)
                {
                    CustomScrollItem item = m_itemList.Last.Value;
                    m_itemList.RemoveLast();
                    item.transform.SetAsFirstSibling();
                    item.Index -= m_viewCount;
                    item.gameObject.SetActive(item.Index >= 0);
                    m_itemList.AddFirst(item);
                }
                deltaX = m_deltaX;
            }
            else
            {

                if (firstItem.Index >= m_maxEndIndex) return;
                for (int i = 0; i < m_runCellGrid.y; i++)
                {
                    CustomScrollItem item = m_itemList.First.Value;
                    m_itemList.RemoveFirst();
                    item.transform.SetAsLastSibling();
                    item.Index += m_viewCount;
                    item.gameObject.SetActive(item.Index < m_maxCellCount);
                    m_itemList.AddLast(item);
                }
                deltaX = -m_deltaX;
            }
            Vector2 currentPos = content.anchoredPosition;
            currentPos.x -= deltaX;
            content.anchoredPosition = currentPos;
            m_ContentStartPosition.x -= deltaX;
            UpdatePrevData();

        }


        #endregion


    }
}



