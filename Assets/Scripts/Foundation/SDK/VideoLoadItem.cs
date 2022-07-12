using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class VideoLoadItem : MonoBehaviour
{
    [SerializeField] private MyButton _btn;
    [SerializeField] private RectTransform _loadTrans;
    private void Awake()
    {
        IAAMgr.Instance.RegisterVideoLoadItem(this);
        LoadingAnimation();
        if (PlatformFactory.Instance.isRewardLoaded())
        {
            gameObject.SetActive(false);
            if(_btn != null)
                _btn.interactable = true;
        }  
        else
        {
            gameObject.SetActive(true);
            if(_btn != null)
                _btn.interactable = false;
        }
                
    }
    void LoadingAnimation()
    {
        _loadTrans.DOLocalRotate(new Vector3(0, 0, -360), 1f,RotateMode.FastBeyond360).SetLoops(-1);
    }
    public void Check(bool isLoad)
    {
        if (isLoad)
        {
            gameObject.SetActive(false);
            if (_btn != null)
                _btn.interactable = true;
        } 
        else
        {
            gameObject.SetActive(true);
            if (_btn != null)
                _btn.interactable = false;
        }
                
    }

    private void OnDestroy()
    {
        IAAMgr.Instance.UnRegisterVideoLoadItem(this);
    }

}


