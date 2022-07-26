using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foundation;


public class IAAMgr : Singleton<IAAMgr>
{
    public int InterStartGameCount { get; set; }
    private List<VideoLoadItem> _videoLoadItems = new List<VideoLoadItem>(10);
    /// <summary>
    /// 注册视频点load
    /// </summary>
    /// <param name="item"></param>
    public void RegisterVideoLoadItem(VideoLoadItem item)
    {
        _videoLoadItems.Add(item);
    }
    /// <summary>
    /// 注销
    /// </summary>
    /// <param name="item"></param>
    public void UnRegisterVideoLoadItem(VideoLoadItem item)
    {
        if (_videoLoadItems.Contains(item))
            _videoLoadItems.Remove(item);
    }
    /// <summary>
    /// 更新所有item状态
    /// </summary>
    /// <param name="isLoad"></param>
    public void NotifyAll(bool isLoad)
    {
            for(int i = 0; i < _videoLoadItems.Count; i++)
        {
            VideoLoadItem item = _videoLoadItems[i];
            if (item == null) continue;
            item.Check(isLoad);
        }
    }

    public void ShowRewardedVideo(string tag,System.Action<bool> callback)
    {
        Time.timeScale = 0;
        PlatformFactory.Instance.showRewardedVideo(tag, (success) =>
        {
            if (callback != null)
                callback(success);
            Time.timeScale = 1;
        });
    }

    public void ShowInterVideo(System.Action<bool> callback)
    {
        PlatformFactory.Instance.showInterAd(callback);
    }
}



