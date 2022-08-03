using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIFramework;

namespace MHSpace
{
    public class UIGamePage : UIPage
    {
        [SerializeField] private MyText _moneyTxt;
        [SerializeField] private List<UIMainItem> _items;
        private void Awake()
        {
            EventDispatcher.Instance.AddListener(EnumEventType.Event_Player_Money, ChangeMoney);
        }
        protected override void InitPage(object args = null)
        {
            ChangeMoney(null);
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Init();
            }
        }
        private void ChangeMoney(BaseEventArgs args)
        {
            if(_moneyTxt !=null)
                _moneyTxt.text = $"{UnitConvertMgr.Instance.GetFloatValue(PlayerMgr.Instance.Player.money, 2)}";
        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener(EnumEventType.Event_Player_Money, ChangeMoney);
        }
    }
}

