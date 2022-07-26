using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space 
{
    public class AutoModel : MonoBehaviour
    {
        private SpriteRenderer _sp;
        private bool _isMove;
        private Vector3 _horizontalDir;
        private Vector3 _verticalDir;
        private Vector3 _lastPos;
        private BallAuto _ballAuto;
        private float _t;

        private bool _isTouch;
        private float _horizontalspeed;
        private float _gravity;
        private int _power;
        private void Awake()
        {
            _sp = this.GetComponent<SpriteRenderer>();
            EventDispatcher.Instance.AddListener(EnumEventType.Event_Game_Next, ReturnPool);
        }
        private void Start()
        {
            //_trail.Init();
        }

        public void Init(Vector3 vec, float angle, BallAuto ballAuto, bool isTouch)
        {
            _sp.enabled = true;
            transform.position = vec;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            _horizontalDir = transform.right;
            _isMove = true;
            //_trail.Activate();
            _ballAuto = ballAuto;
            _t = 0;
            _isTouch = isTouch;
            _gravity = _ballAuto.gravity;
            _horizontalspeed = _ballAuto.horizontalspeed ;
            if (_isTouch)
                _horizontalspeed = Random.Range(0, _horizontalspeed * 0.5f);
            if (Random.Range(0, 2) == 0)
                _horizontalspeed = -_horizontalspeed;
            _verticalDir = (MapCtrl.Instance.CenterPos - transform.position).normalized;
            _horizontalDir = Vector3.Cross(_verticalDir, Vector3.forward);  //外积（叉积）
            _power = (int)_ballAuto.radius;
        }

        private void FixedUpdate()
        {
            if (!_isMove) return;
            _t += Time.fixedDeltaTime;
            _verticalDir = (MapCtrl.Instance.CenterPos - transform.position).normalized;
            _horizontalDir = Vector3.Cross(_verticalDir, Vector3.forward);  //外积（叉积）
            for (int i = 0; i < 5; i++)
            {
                //transform.Translate(_horizontalDir.normalized * _ballAuto.horizontalspeed * Time.deltaTime * 0.2f, Space.World);
                transform.Translate(_horizontalDir.normalized * _horizontalspeed * Time.deltaTime * 0.2f, UnityEngine.Space.World);
                transform.Translate(_verticalDir * _gravity * _t * Time.fixedDeltaTime * 0.2f, UnityEngine.Space.World);
                transform.rotation = Quaternion.FromToRotation(Vector3.up, transform.position - _lastPos);
                _lastPos = transform.position;
                EventDispatcher.Instance.TriggerEvent(new EventArgsThree<Vector3, float, System.Action>(EnumEventType.Event_Cell_EraseMask, transform.position, _power, EraseMask));
            }
        }
        private void OnTriggerCallBack()
        {
            ReturnPool();
        }
        /// <summary>
        /// 擦除
        /// </summary>
        protected virtual void EraseMask()
        {
            ReturnPool();
        }

        /// <summary>
        ///  基地挂了，回收球
        /// </summary>
        public void ReturnPool()
        {
            _isMove = false;
            //_trail.StopSmoothly(0.3f);
            _sp.enabled = false;
            Foundation.Timer.Instance.Register(0.3f, (para) =>
            {
                if (gameObject.activeSelf)
                    SG.ResourceManager.Instance.ReturnObjectToPool(gameObject);
            }).AddTo(gameObject);

        }
        private void OnDisable()
        {
            //_trail.Deactivate();
        }
        private void ReturnPool(BaseEventArgs args)
        {
            ReturnPool();
        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener(EnumEventType.Event_Game_Next, ReturnPool);
        }
    }
}


