using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework
{
    public enum RedDotType
    {
        None = 0,
        Turn=1,//转盘
        DailyLogin=2,//签到-每日签到
        FirstWeek=3,//签到-首周奖励
        TaskDaily=4,//任务-每日
        TaskAchievement=5,//任务-成就
        CompleteAllDailyTask=6,//完成所有每日任务
        AllPrecisionReward = 7,//专精度奖励
        SinglePrecisionReward=8,//单个皮肤专精度奖励
    }
}

