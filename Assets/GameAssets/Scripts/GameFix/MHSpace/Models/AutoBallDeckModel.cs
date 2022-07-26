using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foundation;
using UnityEngine.EventSystems;
using libx;

namespace MHSpace
{
    public class AutoBallDeckModel : BaseTouch
    {
        private BallAuto _ballAuto;
        private PointerEventData _eventData;
        private float _time;
        private float _cd;
        private float _currentCd;
        private Queue<Vector3> _touchPos = new Queue<Vector3>();
        private void Awake()
        {
            
        }
        public override void Start()
        {
            base.Start();
            _eventData = new PointerEventData(EventSystem.current);
            _ballAuto = BallMgr.Instance.m_ballAuto;
            _cd = _ballAuto.cd;
            _currentCd = _cd;
        }
        
        public override void Update()
        {
            if (GameCtrl.Instance.GetGameState() != GameState.start) return;
            base.Update();
            _time += Time.deltaTime;
            if (_time >= _currentCd)
            {
                Fire();
                _time = 0;
            }
        }
        public override void OnTouchEnded(Vector3 touchPos)
        {
            if (GameCtrl.Instance.GetGameState() != GameState.start) return;
            if (PointerOverUI(touchPos))
                return;
            if (_touchPos.Count <= 0)
            {
                Vector3 vec = Camera.main.ScreenToWorldPoint(touchPos);
                _touchPos.Enqueue(new Vector3(vec.x, vec.y, 0));
                object[] objs = new object[] { touchPos };
            }

        }
        private bool PointerOverUI(Vector2 mousePosition)
        {
            _eventData.position = mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            //向点击位置发射一条射线，检测是否点击UI
            EventSystem.current.RaycastAll(_eventData, results);
            if (results.Count > 0)
                return true;
            return false;
        }
        private void Fire()
        {
            float angle;
            Vector3 pos;
            bool isTouch;
            if (_touchPos.Count > 0)
            {
                Vector3 touchPos = _touchPos.Dequeue();
                Vector3 direction = (touchPos - MapCtrl.Instance.CenterPos).normalized;
                angle = Quaternion.FromToRotation(Vector3.up, direction).eulerAngles.z;
                pos = Quaternion.Euler(0, 0, angle) * Vector3.up * 9 + MapCtrl.Instance.CenterPos;
                isTouch = true;
            }
            else
            {
                pos = MapCtrl.Instance.RandomPos(out angle, 9);
                isTouch = false;
            }
            //GameObject prefab = AssetLoader.Load<GameObject>("auto.prefab");
            //GameObject ballObj = GameObject.Instantiate<GameObject>(prefab);
            GameObject ballObj = SG.ResourceManager.Instance.GetObjectFromPool("auto.prefab", true, 1);
            
            AutoModel model = ballObj.GetOrAddComponent<AutoModel>();
            model.Init(pos, angle, _ballAuto, isTouch);

        }
        private void OnDestroy()
        {
            
        }
    }
}

