using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foundation;

namespace MHSpace
{
    public class GameLauncher : MonoBehaviour
    {
        public void Init()
        {
            LogUtility.LogInfo($"GameLauncher Init");
//#if !UNITY_EDITOR || !LOG_ENABLE
//            Debug.unityLogger.logEnabled = false;
//#endif
            InitLocalData();
            GameCtrl.Instance.Init();
        }
        private void InitLocalData()
        {
            MapMgr.Instance.Init();
            BallMgr.Instance.Init();
        }
    }
}

