using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace UIFramework
{
    [RequireComponent(typeof(Toggle))]
    public class ToggleEx : MonoBehaviour
    {
        [SerializeField]
        private List<ColorGraphic> _colorGraphics;
        private void Awake()
        {
            Toggle toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnToggleValueChange);
            OnToggleValueChange(toggle.isOn);
        }

        private void OnToggleValueChange(bool isOn)
        {
            if (_colorGraphics == null || _colorGraphics.Count <= 0) return;
            for(int i=0;i<_colorGraphics.Count;i++)
            {
                _colorGraphics[i].SetColor(isOn);
            }
        }

    }

    [System.Serializable]
    public class ColorGraphic
    {
        public Graphic _graphic;
        public Color _activeColor;
        public Color _unactiveColor;

        public void SetColor(bool isOn)
        {
            if (_graphic == null) return;
            _graphic.color = isOn ? _activeColor : _unactiveColor;
        }
    }
}

