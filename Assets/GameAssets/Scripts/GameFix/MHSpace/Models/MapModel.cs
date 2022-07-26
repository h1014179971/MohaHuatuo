using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libx;
using Foundation;
using DG.Tweening;

namespace MHSpace
{
    public class MapModel : MonoBehaviour
    {
        private Transform _bg;
        private SpriteRenderer _cellSp;
        private Map _map;
        private Tweener _cellTweener;
        private void Awake()
        {
            _bg = this.GetComponentByPath<Transform>("bg");
            _cellSp = this.GetComponentByPath<SpriteRenderer>("function/cells");
        }
        public void Init(Map map)
        {
            _map = map;
            Texture2D bigTex = LoadSprite();
            Debug.Log($"LoadCenter");
            Texture2D centerTex = LoadCenter();
            bigTex = TextureScale.ComplexTwoTextures(bigTex, centerTex, false);
            GameObject obj = new GameObject();
            obj.transform.SetParent(_cellSp.transform);
            obj.transform.localPosition = Vector3.zero;
            CellModel cellModel = obj.GetOrAddComponent<CellModel>();
            cellModel.Init(bigTex, 10, centerTex.width, centerTex.height, CellType.general);

            StartCoroutine(WaitDrawMap());

        }
        private Texture2D LoadSprite()
        {
            string cellInfo = _map.cellInfo;
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

            
        }
        IEnumerator WaitDrawMap()
        {
            yield return new WaitForEndOfFrame();
            //EventDispatcher.Instance.TriggerEvent(new BaseEventArgs(EnumEventType.Event_Game_Start));
            BgAnimation();
            CellsAnimation();
        }
        private void BgAnimation()
        {
            for (int i = 0; i < _bg.childCount; i++)
            {
                int index = i;
                Transform child = _bg.Find($"halo{index}");
                child.DOLocalRotate(Vector3.back * 360, 180 - 60 * index, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
            }
        }
        private void CellsAnimation()
        {
            _cellTweener = _cellSp.transform.DOLocalRotate(Vector3.back * 360, 60, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
        }
    }
}

