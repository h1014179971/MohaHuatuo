using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space
{
    /// <summary>
    /// 游戏状态
    /// </summary>
    [System.Serializable]
    public enum GameState
    {
        none,
        waitStart,//游戏开始之前
        start,
        next,
        exit,//游戏退出
        over,
        win,
        lose,
    }
    /// <summary>
    /// 地块类型
    /// </summary>
    [System.Serializable]
    public enum CellType
    {
        none,
        general,
        center,
    }
}
