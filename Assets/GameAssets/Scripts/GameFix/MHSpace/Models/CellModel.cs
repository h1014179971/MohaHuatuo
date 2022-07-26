using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHSpace
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
        private List<Vector2Int> _circleVecs = new List<Vector2Int>(10000);
        private Color[] _texColors;
        private Texture2D _maskTex;//擦除的图片
        private Color[] _maskTexColors;
        private bool _isDeath;
        private class PixelMask
        {
            public Vector2Int pixelCenter;//中心点
            public int radius;//范围半径
            public Dictionary<Vector2Int, Color> pixelDict;
        }
        private Stack _pixelMaskStack;
        private List<PixelMask> _pixelMasks = new List<PixelMask>();
        private void Awake()
        {
            _sp = this.GetOrAddComponent<SpriteRenderer>();
            EventDispatcher.Instance.AddListener(EnumEventType.Event_Cell_EraseMask, EraseMask);
            _pixelMaskStack = new Stack();
            _maskTex = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            _maskTexColors = new Color[3000];
            for (int i = 0; i < 100; i++)
            {
                PixelMask pixelMask = new PixelMask();
                pixelMask.pixelDict = new Dictionary<Vector2Int, Color>(10000);
                _pixelMasks.Add(pixelMask);
            }
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
            _texColors = _tex.GetPixels();
        }
        /// <summary>
        /// 擦除对应区域
        /// </summary>
        /// <param name="args"></param>
        private void EraseMask(BaseEventArgs args)
        {
            EventArgsThree<Vector3, float, System.Action> arg = args as EventArgsThree<Vector3, float, System.Action>;
            if (EraseMask(arg.param1, arg.param2))
                arg.param3?.Invoke();
        }
        /// <summary>
        /// 擦除区域
        /// </summary>
        /// <param name="contactPos"></param>
        /// <param name="rd"></param>
        /// <param name="tex"></param>
        /// <param name="AlphaTrigger">if true ,接触点为透明，依然做擦除处理</param>
        /// <returns></returns>
        private bool EraseMask(Vector3 contactPos, float rd, Texture2D tex = null, bool AlphaTrigger = false)
        {
            Vector2Int pixel = Vector2Int.zero;
            if (!AlphaTrigger)
            {
                if (!OnTrigger(contactPos, ref pixel)) return false;
            }
            else
                OnTrigger(contactPos, ref pixel);

            _circleVecs.Clear();
            Color color = _tex.GetPixel(pixel.x, pixel.y);
            color.a = 0;
            bool isIncenter = false;
            int radius = (int)(rd * m_scaleMul);
            List<Vector2Int> vecs = Circle(pixel.x, pixel.y, radius, ref isIncenter);
            PixelMask pixelMask;
            if (_pixelMasks.Count <= 0)
            {
                pixelMask = new PixelMask();
                pixelMask.pixelDict = new Dictionary<Vector2Int, Color>(10000);
                _pixelMasks.Add(pixelMask);
            }
            else
                pixelMask = _pixelMasks[0];
            pixelMask.pixelCenter = pixel;
            pixelMask.radius = radius;
            if (tex != null)
            {
                tex.Resize((int)radius * 2, (int)radius * 2, TextureFormat.ARGB32, false);
                Color cl = Color.white;
                cl.a = 0;
                //_maskTexColors = tex.GetPixels();
                if (tex.width * tex.height > _maskTexColors.Length)
                    _maskTexColors = new Color[tex.width * tex.height];
                for (int c = 0; c < _maskTexColors.Length; c++)
                    _maskTexColors[c] = cl;
                tex.SetPixels(_maskTexColors);
            }


            Vector2Int tv = Vector2Int.zero;
            int index, index1;
            for (int i = 0; i < vecs.Count; i++)
            {
                Vector2Int vec = vecs[i];
                //color = _tex.GetPixel(vec.x, vec.y);
                index = vec.y * m_Width + vec.x;
                if (index >= _texColors.Length) continue;
                color = _texColors[index];
                if (color.a != 0)
                {
                    pixelMask.pixelDict[vec] = color;
                }
                if (tex != null)
                {
                    tv.x = vec.x - pixel.x + radius;
                    tv.y = vec.y - pixel.y + radius;
                    if (tv.x >= 0 && tv.x < tex.width && tv.y >= 0 && tv.y < tex.height)
                    {
                        index1 = tv.y * tex.width + tv.x;
                        if (_maskTexColors[index1].a == 0 && color.a != 0)
                            _maskTexColors[index1] = color;
                    }

                }
                color.a = 0;
                _texColors[index] = color;
            }
            if (tex != null)
                tex.SetPixels(_maskTexColors);
            _tex.SetPixels(_texColors);
            if (tex != null)
                tex.Apply();
            if (pixelMask.pixelDict.Count > 0)
            {
                _pixelMaskStack.Push(pixelMask);
                _pixelMasks.Remove(pixelMask);
            }

            _tex.Apply();

            if (isIncenter && !_isDeath)
            {
                _isDeath = true;
                Time.timeScale = 0.3f;
                Foundation.Timer.Instance.Register(0.3f, (para) =>
                {
                    EventDispatcher.Instance.TriggerEvent(new BaseEventArgs(EnumEventType.Event_Game_Next));
                }).AddTo(gameObject);

            }

            return true;
        }
        /// <summary>
        /// 判断pos位置是否在图片上
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool OnTrigger(Vector3 pos, ref Vector2Int pixel)
        {
            Vector3 localPos = _sp.transform.InverseTransformPoint(pos);
            int pixelX = (int)(localPos.x / FixScreen.unit) + m_Width / 2;
            int pixelY = (int)(localPos.y / FixScreen.unit) + m_Height / 2;
            if (pixelX < 0 || pixelX >= m_Width || pixelY < 0 || pixelY >= m_Height || _tex == null) return false;
            Color color = _tex.GetPixel(pixelX, pixelY);
            pixel.x = pixelX;
            pixel.y = pixelY;
            if (color.a == 0) return false;
            return true;
        }
        private List<Vector2Int> Circle(int centerX, int centerY, int radius, ref bool isIncenter)
        {
            //List<Vector2Int> vecs = new List<Vector2Int>();
            //int minx = centerX - radius;
            //minx = Mathf.Max(0, minx);
            //int maxx = centerX + radius;
            //maxx = Mathf.Min(maxx, m_Width);
            Vector2Int vec = Vector2Int.zero;
            for (int x = 0; x <= radius; x++)
            {
                int y = Mathf.FloorToInt(Mathf.Sqrt(radius * radius - x * x));
                for (int i = 0; i <= y; i++)
                {
                    //Vector2Int vec = new Vector2Int(x, i + centerY);
                    vec.x = x + centerX;
                    vec.y = i + centerY;
                    //if (!_circleVecs.Contains(vec))
                    if (vec.x >= 0 && vec.x < m_Width && vec.y >= 0 && vec.y < m_Height)
                    {
                        _circleVecs.Add(vec);
                        if (!isIncenter)
                            isIncenter = InCenter(vec);
                    }

                    vec.x = -x + centerX;
                    vec.y = i + centerY;
                    //if (!_circleVecs.Contains(vec))
                    if (vec.x >= 0 && vec.x < m_Width && vec.y >= 0 && vec.y < m_Height)
                    {
                        _circleVecs.Add(vec);
                        if (!isIncenter)
                            isIncenter = InCenter(vec);
                    }

                    vec.x = x + centerX;
                    vec.y = -i + centerY;
                    //if (!_circleVecs.Contains(vec))
                    if (vec.x >= 0 && vec.x < m_Width && vec.y >= 0 && vec.y < m_Height)
                    {
                        _circleVecs.Add(vec);
                        if (!isIncenter)
                            isIncenter = InCenter(vec);
                    }

                    vec.x = -x + centerX;
                    vec.y = -i + centerY;
                    //if (!_circleVecs.Contains(vec))
                    if (vec.x >= 0 && vec.x < m_Width && vec.y >= 0 && vec.y < m_Height)
                    {
                        _circleVecs.Add(vec);
                        if (!isIncenter)
                            isIncenter = InCenter(vec);
                    }

                }

            }
            return _circleVecs;
        }
        private bool InCenter(Vector2Int vec)
        {
            int x = m_Width / 2 - vec.x;
            int y = m_Height / 2 - vec.y;
            if (x * x + y * y <= (m_centerWidth * m_centerWidth) / 4)//判断是否在中心点圆内
                return true;
            return false;
        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.AddListener(EnumEventType.Event_Cell_EraseMask, EraseMask);
        }
    }
}

