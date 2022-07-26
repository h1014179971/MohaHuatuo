using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EditorPlatform : PlatformFactory
{
    public override void initSDK()
    {
        Debug.Log("EditorPlatform initSDK");
    }
    public override bool isRewardLoaded()
    {
        Debug.Log("EditorPlatform isRewardLoaded");
        return true;
    }
    public override void showRewardedVideo(string tag)
    {
        Debug.Log("EditorPlatform showRewardedVideo");
    }
    public override void showRewardedVideo(string tag, Action<bool> actionCallBack)
    {
        Debug.Log("EditorPlatform showRewardedVideo Action");
        actionCallBack(true);
    }
    public override bool isInterLoaded()
    {
        Debug.Log("EditorPlatform isInterLoaded");
        return true;
    }
    public override void showInterAd()
    {
        Debug.Log("EditorPlatform showInterAd");
    }

    public override void showInterAd(Action<bool> actionCallBack)
    {
        Debug.Log("EditorPlatform showInterAd action");
        actionCallBack?.Invoke(true);
    }
    public override void GameQuit()
    {
        Application.Quit();
    }
    public override void TAEventPropertie(string key, Dictionary<string, string> dic)
    {
        string jsonStr = FullSerializerAPI.Serialize(typeof(Dictionary<string, string>), dic);
        TAEventPropertie(key, jsonStr);
    }
    public override void TAEventPropertie(string key, string jsonStr)
    {
        Debug.Log("AndroidPlatform EventPropertie:" + jsonStr);
    }

}
