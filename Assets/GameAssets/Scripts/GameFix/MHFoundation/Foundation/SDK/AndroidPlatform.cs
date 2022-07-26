#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


public class AndroidPlatform : PlatformFactory
{
                                                               
    public override void initSDK()
    {
        AndroidPlatformWrapper.Instance.initSDK();
    }
    public override bool isRewardLoaded()
    {
        Debug.Log("AndroidPlatform isRewardLoaded");
        return AndroidPlatformWrapper.Instance.isRewardLoaded();
    }
    public override void showRewardedVideo(string tag)
    {
        Debug.Log("AndroidPlatform showRewardedVideo");
        AndroidPlatformWrapper.Instance.showRewardedVideo(tag);
    }
    public override void showRewardedVideo(string tag, Action<bool> actionCallBack)
    {
        Debug.Log("AndroidPlatform showRewardedVideo Action");
        AndroidPlatformWrapper.Instance.showRewardedVideo(tag,actionCallBack);
    }
    public override bool isInterLoaded()
    {
        Debug.Log("AndroidPlatform isInterLoaded");
        return AndroidPlatformWrapper.Instance.isInterLoaded();
    }
    public override void showInterAd()
    {
        Debug.Log("AndroidPlatform showInterAd");
        AndroidPlatformWrapper.Instance.showInterAd();
    }

    public override void showInterAd(Action<bool> actionCallBack)
    {
        Debug.Log("AndroidPlatform showInterAd Action");
        AndroidPlatformWrapper.Instance.showInterAd(actionCallBack);
    }
    public override void GameQuit()
    {
        AndroidPlatformWrapper.Instance.GameQuit();
    }
    public override void TAEventPropertie(string key, Dictionary<string, string> dic)
    {
        string jsonStr = FullSerializerAPI.Serialize(typeof(Dictionary<string, string>), dic);
        TAEventPropertie(key, jsonStr);
    }
    public override void TAEventPropertie(string key, string jsonStr)
    {
        Debug.Log("AndroidPlatform EventPropertie:" + jsonStr);
        AndroidPlatformWrapper.Instance.TAEventPropertie(key,jsonStr);
    }

}
#endif
