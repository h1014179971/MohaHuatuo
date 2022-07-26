using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space
{
    [System.Serializable]
    public class Map
    {
        public readonly int id;
        public readonly string aiInfo;
        public readonly string prefabName;
        public readonly string cellInfo;
        public readonly string centerInfo;
        public readonly MapType mapType;
        public readonly string erasePrice;
        public readonly float eraseMul;
        public readonly int deckMaxLv;
        public readonly float addlvMul;
        public readonly float addMax;
        public readonly float cutlvMul;
        public readonly float cutMax;
        public readonly string moneyReward;
        public readonly string diamondReward;
        public readonly string bossSkillIds;//关卡技能
    }
    [System.Serializable]
    public enum MapType
    {
        none,
        general,    //普通关卡
        boss,       //boss关卡
    }
}

