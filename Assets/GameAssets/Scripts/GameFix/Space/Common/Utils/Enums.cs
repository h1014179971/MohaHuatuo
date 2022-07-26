using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space
{
    /// <summary>
    /// ��Ϸ״̬
    /// </summary>
    [System.Serializable]
    public enum GameState
    {
        none,
        waitStart,//��Ϸ��ʼ֮ǰ
        start,
        next,
        exit,//��Ϸ�˳�
        over,
        win,
        lose,
    }
    /// <summary>
    /// �ؿ�����
    /// </summary>
    [System.Serializable]
    public enum CellType
    {
        none,
        general,
        center,
    }
}
