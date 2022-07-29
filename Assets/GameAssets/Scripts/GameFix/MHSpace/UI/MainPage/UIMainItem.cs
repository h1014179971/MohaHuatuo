using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foundation;

namespace MHSpace
{
    public class UIMainItem : MonoBehaviour
    {
        [SerializeField]private MyText _lvTxt;
        [SerializeField]private MyText _priceTxt;
        [SerializeField]private PowerType _powerType;
        private int _currentLv;//当前等级
        private PowerInfo _currentPowerInfo;
        private void Awake()
        {
            MyButton btn = this.GetComponent<MyButton>();
            btn.onClick.AddListener(OnClick);
        }
        public void Init()
        {
            _currentLv = PlayerMgr.Instance.GetPowerLvByType(_powerType);
            InitData();
        }
        private void InitData()
        {
            _lvTxt.text = _currentLv.ToString();
            _currentPowerInfo = PowerInfoMgr.Instance.GetPowerByTypeLv(_powerType, _currentLv);
            _priceTxt.text = $"{UnitConvertMgr.Instance.GetFloatValue(new Long2(_currentPowerInfo.price), 2)}";
        }

        private void OnClick()
        {
            if (PowerInfoMgr.Instance.IsMaxPowerLv(_powerType, _currentLv)) return;
            if (new Long2(_currentPowerInfo.price) > PlayerMgr.Instance.Player.money) return;
            _currentLv++;
            PlayerMgr.Instance.Player.powerDict[_powerType] = _currentLv;
            PlayerMgr.Instance.CutMoney(new Long2(_currentPowerInfo.price));
            InitData();
        }
    }
}

