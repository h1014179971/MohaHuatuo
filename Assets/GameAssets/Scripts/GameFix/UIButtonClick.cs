using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Moha.HotFix
{
    public class UIButtonClick : MonoBehaviour
    {
        [SerializeField]private Text _btnTxt;
        void Start()
        {
           var btn =  this.GetComponent<Button>();
            btn.onClick.AddListener(() =>
            {
                _btnTxt.text = "success";
            });
        }

        
    }
}

