using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space
{
    public class CellModel : MonoBehaviour
    {
        private SpriteRenderer _sp;
        private Texture2D _tex;
        public int m_Width;
        public int m_Height;
        public float m_scaleMul;
        private int m_centerWidth;
        private int m_centerHeight;
        private void Awake()
        {
            _sp = this.GetOrAddComponent<SpriteRenderer>();
        }
        public void Init(Texture2D tex, int index, int centerWidth, int centerHeight, CellType cellType)
        {
            _tex = tex;
            m_Width = _tex.width;
            m_Height = _tex.height;
            m_scaleMul = 1;
            _sp.sprite = Sprite.Create(_tex, new Rect(0, 0, m_Width, m_Height), Vector2.one * 0.5f);
            m_centerWidth = centerWidth;
            m_centerHeight = centerHeight;
            transform.localScale = Vector3.one;
            _sp.sortingOrder = index;
            _tex.Apply();
            //_cellType = cellType;
            transform.name = $"cell_{index}";
            //MapCtrl.Instance.AddCellModel(this);
        }
    }
}

