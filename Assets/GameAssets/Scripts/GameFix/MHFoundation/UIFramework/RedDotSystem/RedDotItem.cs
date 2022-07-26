using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    public class RedDotItem : MonoBehaviour
    {
        /// <summary>
        /// 红点物体;
        /// </summary>
        [Header("红点")]
        public Transform _RedDot;
        public bool _isStartCheck;
        /// <summary>
        /// 红点类型;
        /// </summary>
        public List<RedDotType> redDotTypes;

        [HideInInspector] public object[] selfDatas = new object[0];
        protected bool bCachedRedDot = false;


        void Awake()
        {
            //注册红点;
            RedDotCtrl.Instance.RegisterRedDot(redDotTypes, this);

        }
        private void Start()
        {
            if (_isStartCheck)
            {
                Check(null);
            }
        }
        public void Check(object[] objs)
        { 
            //设置红点;
            bool bRedDot = RedDotCtrl.Instance.Check(redDotTypes, objs, selfDatas);
            SetData(bRedDot, true);
        } 

        void OnDestroy()
        {
            //取消注册红点;
            RedDotCtrl.Instance.UnRegisterRedDot(this);
        }

        /// <summary>
        /// 设置红点显示;
        /// </summary>
        /// <param name="bRedDot"></param>
        public void SetData(bool bRedDot, bool bForceRefresh = false)
        {
            if (bForceRefresh)
            {
                //_RedDot.enabled = bRedDot;
                _RedDot.gameObject.SetActive(bRedDot);
                bCachedRedDot = bRedDot;
                return;
            }

            if (bCachedRedDot != bRedDot)
            {
                //_RedDot.enabled = bRedDot;
                _RedDot.gameObject.SetActive(bRedDot);
                bCachedRedDot = bRedDot;
            }
        }

        public void SetSelfData(object[] args = null)
        {
            if (args != null)
            {
                selfDatas = args;
            }
        }
        /// <summary>
        /// 获取当前物体挂载的所有红点;
        /// </summary>
        /// <returns></returns>
        public List<RedDotType> GetRedDotTypes()
        {
            return this.redDotTypes;
        }

    }
}

