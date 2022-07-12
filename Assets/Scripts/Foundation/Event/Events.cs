using UnityEngine;

[System.Serializable]
public enum EnumEventType
{
    None,
    Event_Player_Money,//玩家钞票改变
    Event_Game_StartWait,//等待开始
    Event_Game_StartDown,//开始倒计时
    Event_Game_Start,//游戏开始
    Event_Game_FirstMove,//开始游戏后第一次移动
    Event_Game_PlayerNumble,//游戏 玩家数量变化
    Event_GameOver,
    Event_Create_Player,
    Event_Character_Add_Plank,
    Event_AI_RemoveTargetPlank,//AI锁定的目标木板被移除
    Event_Handbook_OnPageChanged,
    Event_Handbook_AddSkinInfo,
    Event_Player_SelectSkin,//玩家当前选择的皮肤改变


    EventTypeEnd
}

[System.Serializable]
public class BaseEventArgs
{
    public EnumEventType eventType;
    
    public BaseEventArgs(EnumEventType type)
    {
        eventType = type;
    }
}

[System.Serializable]
public class EventArgsOne<T> : BaseEventArgs
{
    public T param1;
    
    public EventArgsOne(EnumEventType type, T p1) : base(type)
    {
        param1 = p1;
    }
}

[System.Serializable]
public class EventArgsTwo<T1, T2> : EventArgsOne<T1>
{
    public T2 param2;

    public EventArgsTwo(EnumEventType type, T1 p1, T2 p2) : base(type, p1)
    {
        param2 = p2;
    }
}

[System.Serializable]
public class EventArgsThree<T1, T2, T3> : EventArgsTwo<T1, T2>
{
    public T3 param3;

    public EventArgsThree(EnumEventType type, T1 p1, T2 p2, T3 p3) : base(type, p1, p2)
    {
        param3 = p3;
    }
}

public delegate void OnTouchEventHandle(GameObject _listener, object _args, params object[] _params);
public enum EnumTouchEventType
{
    OnClick,
    OnDoubleClick,
    OnDown,
    OnUp,
    OnEnter,
    OnExit,
    OnSelect,
    OnUpdateSelect,
    OnDeSelect,
    OnDrag,
    OnDragEnd,
    OnDrop,
    OnScroll,
    OnMove,
}
