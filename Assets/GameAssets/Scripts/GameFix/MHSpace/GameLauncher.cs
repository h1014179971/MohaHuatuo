using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foundation;
using UIFramework;

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
            EventDispatcher.Instance.AddListener(EnumEventType.Event_Load_PlayerData, LoadPlayerData);
            PlayerMgr.Instance.Init();
        }
        private void LoadPlayerData(BaseEventArgs args)
        {
            InitLocalData();

            GameCtrl.Instance.Init();
            UIController.Instance.OpenPageFromAssets<UIGamePage>(UIPrefab.UI_GamePage);
        }
        private void InitLocalData()
        {
            MapMgr.Instance.Init();
            BallMgr.Instance.Init();
            PowerInfoMgr.Instance.Init();
            UnitConvertMgr.Instance.Init();
        }
        private void OnApplicationFocus(bool focus)
        {
            Debug.Log($"OnApplicationFocus:{focus}");
        }
        private void OnApplicationPause(bool pause)
        {
            //ture 为切换到后台
            Debug.Log($"OnApplicationPause:{pause}");
            if (pause)
                PlayerMgr.Instance.RecordPlayer();
            
        }

        private void OnApplicationQuit()
        {
            Debug.Log($"OnApplicationQuit:");
            PlayerMgr.Instance.RecordPlayer();
        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener(EnumEventType.Event_Load_PlayerData, LoadPlayerData);
        }
    }
}

