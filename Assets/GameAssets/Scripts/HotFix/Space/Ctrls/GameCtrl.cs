using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space 
{
    public class GameCtrl : MonoSingleton<GameCtrl>
    {
        protected override void Awake()
        {
            
        }
        public void Init(int lv = -1)
        {
            lv = 1;
            Map map = MapMgr.Instance.GetMapById(lv);
            MapCtrl.Instance.Init(map);
        }
    }
}


