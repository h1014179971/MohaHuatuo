using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libx;
using Foundation;

namespace Space
{
    public class MapModel : MonoBehaviour
    {
        private Transform _bg;
        private SpriteRenderer _cellSp;
        private Map _map;
        private void Awake()
        {
            _bg = this.GetComponentByPath<Transform>("bg");
            _cellSp = this.GetComponentByPath<SpriteRenderer>("function/cells");
        }
        public void Init(Map map)
        {
            _map = map;
            Debug.Log($"LoadSprite");
            Texture2D tex = LoadS();
            return;
            Texture2D bigTex = LoadSprite();
            Debug.Log($"LoadCenter");
            Texture2D centerTex = LoadCenter();
            bigTex = TextureScale.ComplexTwoTextures(bigTex, centerTex, false);
            GameObject obj = new GameObject();
            obj.transform.SetParent(_cellSp.transform);
            obj.transform.localPosition = Vector3.zero;
            CellModel cellModel = obj.GetOrAddComponent<CellModel>();
            cellModel.Init(bigTex, 10, centerTex.width, centerTex.height, CellType.general);

            //StartCoroutine(WaitDrawMap());

        }
        private Texture2D LoadS()
        {
            Debug.Log($"loads");
            string cellInfo = _map.cellInfo;
            Debug.Log($"LoadS:{cellInfo}");
            string[] cells = cellInfo.Split(';');
            Debug.Log($"LoadS11:{cells.Length}");
            Texture2D bigTex = null;
            for (int i = 0; i < cells.Length; i++)
            {
                int index = i;
                string[] cell = cells[i].Split('-');
                Texture2D sprite = AssetLoader.Load<Texture2D>($"{cell[0]}.png");
                float scaleMul = float.Parse(cell[1]);
                Color hexColor = Color.white;
                if (!cell[2].Equals("1"))
                    hexColor = Utils.HexToColor(cell[2]);
                Utils.HexToColor("ffffff");
                Texture2D tex = TextureTest.PointTest(sprite,(int)(sprite.width * scaleMul), (int)(sprite.height * scaleMul), hexColor);
                Debug.Log($"3333333333");
                //Texture2D tex = TextureScale.ScaleTextureBilinear(sprite, (int)(sprite.width * scaleMul), (int)(sprite.height * scaleMul));
                //Debug.Log($"mapModel 1111");
                //Texture2D tex = TextureScaleThread.Bilinear(sprite, (int)(sprite.width * scaleMul), (int)(sprite.height * scaleMul), hexColor);
                //if (index == 0)
                //    bigTex = tex;
                //else
                //    bigTex = TextureScale.ComplexTwoTextures(bigTex, tex, false);
            }

            return bigTex;
        }
        private Texture2D LoadSprite()
        {
            Debug.Log($"LoadSprite1111");
            string cellInfo = _map.cellInfo;
            Debug.Log($"LoadSprite222:{cellInfo}");
            string[] cells = cellInfo.Split(';');
            Texture2D bigTex = null;
            for (int i = 0; i < cells.Length; i++)
            {
                int index = i;
                string[] cell = cells[i].Split('-');
                Texture2D sprite = AssetLoader.Load<Texture2D>($"{cell[0]}.png");
                float scaleMul = float.Parse(cell[1]);
                Color hexColor = Color.white;
                if (!cell[2].Equals("1"))
                    hexColor = Utils.HexToColor(cell[2]);
                //Texture2D tex = TextureScale.ScaleTextureBilinear(sprite, (int)(sprite.width * scaleMul), (int)(sprite.height * scaleMul));
                Debug.Log($"mapModel 1111");
                Texture2D tex = TextureScaleThread.Bilinear(sprite, (int)(sprite.width * scaleMul), (int)(sprite.height * scaleMul), hexColor);
                if (index == 0)
                    bigTex = tex;
                else
                    bigTex = TextureScale.ComplexTwoTextures(bigTex, tex, false);
            }

            return bigTex;
        }
        private Texture2D LoadCenter()
        {
            string centerInfo = _map.centerInfo;
            string[] cell = centerInfo.Split('-');
            Texture2D sprite = AssetLoader.Load<Texture2D>($"{cell[0]}.png");
            float scaleMul = float.Parse(cell[1]);
            Color hexColor = Color.white;
            if (!cell[2].Equals("1"))
                hexColor = Utils.HexToColor(cell[2]);
            //Texture2D tex = TextureScale.ScaleTextureBilinear(sprite, (int)(sprite.width * scaleMul), (int)(sprite.height * scaleMul));
            Texture2D tex = TextureScaleThread.Bilinear(sprite, (int)(sprite.width * scaleMul), (int)(sprite.height * scaleMul), hexColor);
            return tex;

            //string centerInfo = _map.centerInfo;
            //GameObject obj = new GameObject();
            //obj.transform.SetParent(_cellSp.transform);
            //obj.transform.localPosition = Vector3.zero;
            //CellModel cellModel = obj.GetOrAddComponent<CellModel>();
            //cellModel.Init(centerInfo, 30, CellType.center);
        }
    }
}

