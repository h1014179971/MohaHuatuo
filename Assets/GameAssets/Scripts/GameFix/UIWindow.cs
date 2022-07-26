using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Moha.HotFix 
{
    public class UIWindow : MonoBehaviour
    {
        [SerializeField]private Text _txt;
        // Start is called before the first frame update
        void Start()
        {
            _txt.text = "moha hotfix";
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}


