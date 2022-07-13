using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace libx
{
#if UNITY_EDITOR
    public class EditorGameLauncher
    {
        public static List<string> _searchPath = new List<string>();
    }
#endif
    public class AssetInit : MonoBehaviour
    {
        [SerializeField]private Updater _updater;
        IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            _updater.Init();
        }
        public void AssetsInitialize()
        {
            StartCoroutine(AssetsInit());
        }
        IEnumerator AssetsInit()
        {
            var init = Assets.Initialize();
            yield return init;
            if (string.IsNullOrEmpty(init.error))
            {
#if UNITY_EDITOR
                for (int i = 0; i < EditorGameLauncher._searchPath.Count; i++)
                {
                    Assets.AddSearchPath(EditorGameLauncher._searchPath[i]);
                }
#else
                for(int i = 0; i < init._seachPath.Count; i++)
                {
                    string seachPath = init._seachPath[i];
                    Assets.AddSearchPath(seachPath);
                }
#endif

                init.Release();
                Debug.Log("start load data");

                EventDispatcher.Instance.TriggerEvent(new BaseEventArgs(EnumEventType.Event_Asset_Init));

            }
            else
            {
                init.Release();
                Debug.LogError($"Assets ³õÊ¼»¯´íÎó:{init.error}");
            }
        }
    }
}

