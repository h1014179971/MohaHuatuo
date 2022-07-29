using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using libx;
using Foundation;

namespace MHSpace
{
    public class PowerInfoMgr : Singleton<PowerInfoMgr>
    {
        private Dictionary<PowerType, List<PowerInfo>> _allPowerDict = new Dictionary<PowerType, List<PowerInfo>>();//每一种类型的所有数据
        public Dictionary<PowerType, List<PowerInfo>> AllPowerDict { get { return _allPowerDict; } }
        private List<PowerInfo> _allPowers = new List<PowerInfo>();
        public List<PowerInfo> AllPowers { get { return _allPowers; } }

        public override void Init()
        {
            ReadFile();
        }

        private void ReadFile()
        {
            TextAsset textAsset = AssetLoader.Load<TextAsset>(Files.powerInfo);
            string jsonStr = textAsset.text;
            List<PowerInfo> powers = FullSerializerAPI.Deserialize(typeof(List<PowerInfo>), jsonStr) as List<PowerInfo>;
            _allPowers = powers;
            for (int i = 0; i < powers.Count; i++)
            {
                PowerInfo power = powers[i];
                List<PowerInfo> powerInfos = null;
                if (!_allPowerDict.TryGetValue(power.type, out powerInfos))
                {
                    powerInfos = new List<PowerInfo>();
                    _allPowerDict[power.type] = powerInfos;
                }
                powerInfos.Add(power);
            }
        }


        public PowerInfo GetPowerByTypeLv(PowerType type, int lv)
        {
            return _allPowerDict[type].Find(item => item.lv == lv);
        }

        public bool IsMaxPowerLv(PowerType type, int lv)
        {
            //var lists = _allPowerDict[type].FindAll(item => item.lv >= lv);
            if (_allPowerDict[type].Count <= lv)
            {
                return true;
            }
            return false;
        }
    }
}
