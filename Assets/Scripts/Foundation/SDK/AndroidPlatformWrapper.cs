#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AOT;

public class AndroidPlatformWrapper : MonoBehaviour {

    static AndroidPlatformWrapper _instance;
    private static System.Action<bool> rewardCallBack;
    private static System.Action<bool> interCallBack;
    AndroidJavaObject jo;
    public delegate void CallbackDelegate(string str);
    public static AndroidPlatformWrapper Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("AndroidPlatformWrapper");
                go.AddComponent<AndroidPlatformWrapper>();
                _instance = go.GetComponent<AndroidPlatformWrapper>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    public void initSDK()
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        try
        {
            jo = new AndroidJavaObject("com.hw.GamePlayer");
            //jo = new AndroidJavaObject("com.hellowd.GamePlayer");
            jo.Call("initHwSDK", currentActivity, "");
        }
        catch (System.Exception e)
        {
            Debug.Log("call java :" + e.Message);
        }
        
    }
    public bool isRewardLoaded()
    {
        if (jo != null)
            return jo.Call<bool>("isHwRewardLoaded");
        return false;
    }
    public void showRewardedVideo(string tag, System.Action<bool> actionCallBack)
    {
        Debug.Log("AndroidPlatformWrapper showRewardedVideo Action");

        rewardCallBack = actionCallBack;
        showRewardedVideo(tag);
    }
    /// <summary>
    /// 播放视频
    /// </summary>
    public void showRewardedVideo(string tag)
    {
        Debug.Log("AndroidPlatformWrapper showRewardedVideo");
        object[] paramArray = new object[3];
        paramArray[0] = tag;
        paramArray[1] = "PlatformCallback_FinishRewardAd";
        paramArray[2] = "PlatformCallback_FailedRewardAd";
        try
        {
            jo.Call("showHwRewardAd", paramArray);
        }
        catch (System.Exception e)
        {
            Debug.Log("call java :" + e.Message);
        }
        
    }
    /// <summary>
    /// 视频播放成功
    /// </summary>
    /// <param name="jsonStr"></param>
    //[MonoPInvokeCallback(typeof(CallbackDelegate))]
    public void PlatformCallback_FinishRewardAd(string jsonStr)
    {
        Debug.Log("AndroidPlatformWrapper PlatformCallback_FinishRewardAd:"+jsonStr);
        if (rewardCallBack != null)
            rewardCallBack(true);
    }
    /// <summary>
    /// 视频播放失败
    /// </summary>
    /// <param name="jsonStr"></param>
    //[MonoPInvokeCallback(typeof(CallbackDelegate))]
    public void PlatformCallback_FailedRewardAd(string jsonStr)
    {
        Debug.Log("AndroidPlatformWrapper PlatformCallback_FailedRewardAd:"+jsonStr);
        if (rewardCallBack != null)
            rewardCallBack(false);
    }
    public bool isInterLoaded()
    {
        if (jo != null)
            return jo.Call<bool>("isHwInterLoaded");
        return false;
    }
    public void showInterAd()
    {
        Debug.Log("AndroidPlatformWrapper showInterAd");
        object[] paramArray = new object[3];
        paramArray[0] = "showInterAd";
        paramArray[1] = "PlatformCallback_FinishInterAd";
        paramArray[2] = "PlatformCallback_FailedInterAd";
        try
        {
            jo.Call("showHwInterAd", paramArray);
        }
        catch (System.Exception e)
        {
            Debug.Log("call java :" + e.Message);
        }
        
    }
    public void showInterAd(System.Action<bool> actionCallBack)
    {
        Debug.Log("AndroidPlatformWrapper showInterAd");
        interCallBack = actionCallBack;
        showInterAd();
    }
    //[MonoPInvokeCallback(typeof(CallbackDelegate))]
    public void PlatformCallback_FinishInterAd(string jsonStr)
    {
        Debug.Log("AndroidPlatformWrapper PlatformCallback_FinishInterAd:"+jsonStr);
        if (interCallBack != null)
            interCallBack(true);
    }
    //[MonoPInvokeCallback(typeof(CallbackDelegate))]
    public void PlatformCallback_FailedInterAd(string jsonStr)
    {
        Debug.Log("AndroidPlatformWrapper PlatformCallback_FailedInterAd:" + jsonStr);
        if (interCallBack != null)
            interCallBack(false);
    }
    [MonoPInvokeCallback(typeof(CallbackDelegate))]
    public void OnLoadRewardResult(string isLoad)
    {
        Debug.Log($"AndroidPlatformWrapper OnLoadRewardResult Action:{isLoad}");
        if (isLoad.Equals("true"))
        {
            IAAMgr.Instance.NotifyAll(true);
        }
        else
        {
            IAAMgr.Instance.NotifyAll(false);
        }

    }
    /// <summary>
    /// 退出游戏
    /// </summary>
    public void GameQuit()
    {
        try
        {
            jo.Call("GameQuit");
        }
        catch (System.Exception e)
        {
            Debug.Log("call java :" + e.Message);
        }
        
    }
    public void TAEventPropertie(string key, string jsonStr)
    {
        Debug.Log("AndroidPlatformWrapper EventPropertie:" + jsonStr);
        object[] paramArray = new object[2];
        paramArray[0] = key;
        paramArray[1] = jsonStr;
        if (jo != null)
            jo.Call("TAEventPropertie", paramArray);
    }
}
#endif
