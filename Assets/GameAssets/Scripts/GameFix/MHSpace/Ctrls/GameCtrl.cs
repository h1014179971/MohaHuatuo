using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MHSpace
{
    public class GameCtrl : MonoSingleton<GameCtrl>
    {
        private GameState _gameState = GameState.none;
        protected override void Awake()
        {
            
        }
        public void Init(int lv = -1)
        {
            lv = 1;
            Map map = MapMgr.Instance.GetMapById(lv);
            MapCtrl.Instance.Init(map);
            AutoBallDeckModel autoBallDeckModel = GameObject.FindObjectOfType<AutoBallDeckModel>();
            if(autoBallDeckModel == null)
            {
                GameObject obj = new GameObject();
                obj.name = "deck";
                autoBallDeckModel = obj.GetOrAddComponent<AutoBallDeckModel>();
            }
            _gameState = GameState.start;
        }
        public GameState GetGameState()
        {
            return _gameState;
        }
    }
}


