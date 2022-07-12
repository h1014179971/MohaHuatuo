using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    public class UIDepth : MonoBehaviour
    {
        [SerializeField] private UIDepthType _depth;
        [SerializeField] private int _sortingOrder = -1;
        [SerializeField] private bool _needRaycaster = true;
        private Canvas _canvas;
        private GraphicRaycaster _rayCaster;
        private void Awake()
        {
            _canvas = gameObject.GetOrAddComponent<Canvas>();
            _canvas.overrideSorting = true;
            if (_sortingOrder < 0)
                _canvas.sortingOrder = (int)_depth;
            else
                _canvas.sortingOrder = _sortingOrder;
            if (_needRaycaster)
            {
                _rayCaster = gameObject.GetOrAddComponent<GraphicRaycaster>();
            }
               
        }

        public UIDepthType Depth
        {
            get { return _depth; }
            set
            {
                _depth = value;
                SetOrder();
            }
        }

        public int SortingOrder
        {
            get { return _sortingOrder; }
            set
            {
                _sortingOrder = value;
                SetOrder();
            }
        }

        private void SetOrder()
        {
            if(_canvas==null)
            {
                _canvas = gameObject.GetOrAddComponent<Canvas>();
                _canvas.overrideSorting = true;
            }
            if (_sortingOrder < 0)
                _canvas.sortingOrder = (int)_depth;
            else
                _canvas.sortingOrder = _sortingOrder;
        }

        public bool NeedRaycaster
        {
            get { return _needRaycaster; }
            set
            {
                _needRaycaster = value;
                if (_needRaycaster && _rayCaster == null)
                    _rayCaster = gameObject.GetOrAddComponent<GraphicRaycaster>();
                else if (!_needRaycaster && _rayCaster != null)
                {
                    Destroy(_rayCaster);
                    _rayCaster = null;
                }
            }
        }

        public void RemoveAll()
        {
            if (_rayCaster != null)
            {
                Destroy(_rayCaster);
                _rayCaster = null;
            }
            if (_canvas!=null)
            {
                Destroy(_canvas);
                _canvas = null;
            }
           
        }

    }

    public enum UIDepthType
    {
        UIPage     = 0,
        UIWindow   = 10,
        UITop      = 20,
        UIPopup    = 30,
        UIBlocker  = 40,
        UIFx       = 50
    }
}

