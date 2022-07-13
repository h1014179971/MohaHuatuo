using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Reflection;
using libx;

public class LoadDll : MonoBehaviour
{
    Assembly gameAss;
    public static TextAsset[] aotDllBytes;
    public static readonly List<string> aotDlls = new List<string>()
            {
                "mscorlib.dll",
                "System.dll",
                "System.Core.dll",// 如果使用了Linq，需要这个
                // "Newtonsoft.Json.dll",
                // "protobuf-net.dll",
                // "Google.Protobuf.dll",
                // "MongoDB.Bson.dll",
                // "DOTween.Modules.dll",
                // "UniTask.dll",
            };
    private void Awake()
    {
        EventDispatcher.Instance.AddListener(EnumEventType.Event_Asset_Init, AssetInitComplete);
    }
    private void AssetInitComplete(BaseEventArgs args)
    {
        LoadGameDll();
        RunMain();
    }
    
    void LoadGameDll()
    {

#if !UNITY_EDITOR
        aotDllBytes = new TextAsset[aotDlls.Count];
        for (int i = 0;i< aotDlls.Count; i++)
        {
            aotDllBytes[i] = AssetLoader.Load(aotDlls[i] + ".bytes", typeof(TextAsset)) as TextAsset;
        }
        TextAsset hotfixDll = AssetLoader.Load("HotFix.dll.bytes", typeof(TextAsset)) as TextAsset;
        gameAss = Assembly.Load(hotfixDll.bytes);
#else
        gameAss = AppDomain.CurrentDomain.GetAssemblies().First(assembly => assembly.GetName().Name == "HotFix");
#endif
    }

    public void RunMain()
    {
        Debug.Log($"RunMain");
        if (gameAss == null)
        {
            UnityEngine.Debug.LogError("dll未加载");
            return;
        }
        var appType = gameAss.GetType("Moha.HotFix.App");
        var mainMethod = appType.GetMethod("Main");
        mainMethod.Invoke(null, null);

        // 如果是Update之类的函数，推荐先转成Delegate再调用，如
        //var updateMethod = appType.GetMethod("Update");
        //var updateDel = System.Delegate.CreateDelegate(typeof(Action<float>), null, updateMethod);
        //updateDel(deltaTime);
    }
    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveListener(EnumEventType.Event_Asset_Init, AssetInitComplete);
    }
}
