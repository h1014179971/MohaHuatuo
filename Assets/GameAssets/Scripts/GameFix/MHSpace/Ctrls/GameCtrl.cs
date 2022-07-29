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
            EventDispatcher.Instance.AddListener(EnumEventType.Event_Game_Start, GameStart);
            EventDispatcher.Instance.AddListener(EnumEventType.Event_Game_Next, GameNext);
        }
        public void Init(int lv = -1)
        {
            lv = PlayerMgr.Instance.Player.pveLv;
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
        private void GameStart(BaseEventArgs args)
        {
            _gameState = GameState.start;
        }
        private void GameNext(BaseEventArgs args)
        {
            if (_gameState != GameState.start)
                return;
            _gameState = GameState.next;
            Time.timeScale = 1;
            OnMapNext();
        }
        private void OnMapNext()
        {
            
                NextMap(null);

        }
        private void NextMap(BaseEventArgs args)
        {
            PlayerMgr.Instance.AddPveLv(1);
            int lv = PlayerMgr.Instance.Player.pveLv;
            Map map = MapMgr.Instance.GetMapById(lv);
            MapCtrl.Instance.Init(map);
            


        }
        public override void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener(EnumEventType.Event_Game_Start, GameStart);
            EventDispatcher.Instance.RemoveListener(EnumEventType.Event_Game_Next, GameNext);
        }
    }
}


