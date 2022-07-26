using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIFramework
{
    public class CustomScrollItem : MonoBehaviour
    {
        private int m_index;
        public int Index
        {
            get
            {
                return m_index;
            }
            set
            {
                m_index = value;
                InitData();
            }
        }

        public virtual void InitData() { }
       

    }

}
