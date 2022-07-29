using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foundation;

namespace MHSpace
{
    public class PlayerMgr : MonoSingleton<PlayerMgr>
    {
        public Player Player { get; set; }
        public void Init()
        {
            ReadFile();
        }
        private void ReadFile()
        {
            StreamFile.ReaderFile(StreamFile.Combine(Files.jsonFolder, Files.player), this, (jsonStr) =>
            {
                if (!string.IsNullOrEmpty(jsonStr))
                {
                    Player = FullSerializerAPI.Deserialize(typeof(Player), jsonStr) as Player;
                }
                else
                {
                    Player = new Player();
                }
                EventDispatcher.Instance.TriggerEvent(new BaseEventArgs(EnumEventType.Event_Load_PlayerData));
            });
        }
        public void AddPveLv(int addLv)
        {
            Player.pveLv += addLv;
        }
        public void AddMoney(string incomeStr)
        {
            Long2 income = new Long2(incomeStr);
            AddMoney(income);
        }
        public void AddMoney(Long2 income)
        {
            Player.money += income;
            EventDispatcher.Instance.TriggerEvent(new BaseEventArgs(EnumEventType.Event_Player_Money));
        }
        public void CutMoney(Long2 price)
        {
            Player.money -= price;
            if (Player.money < Long2.zero)
                Player.money = Long2.zero;
            EventDispatcher.Instance.TriggerEvent(new BaseEventArgs(EnumEventType.Event_Player_Money));
        }
        public int GetPowerLvByType(PowerType powerType)
        {
            if (!Player.powerDict.ContainsKey(powerType))
                Player.powerDict[powerType] = 1;
            return Player.powerDict[powerType];
        }
        public void RecordPlayer()
        {
            if (Player == null) return;
            string str = FullSerializerAPI.Serialize(typeof(Player), Player, false, false);
            StreamFile.RecordFile(str, StreamFile.Combine(Files.jsonFolder, Files.player));
        }
    }
}

