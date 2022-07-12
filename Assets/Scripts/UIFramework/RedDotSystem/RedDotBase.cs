using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFramework 
{
    public abstract class RedDotBase
    {
        /// <summary>
        /// 是否显示红点(true表示显示,false表示不显示;)
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public virtual bool ShowRedDot(object[] objs, object[] selfObjes)
        {
            return false;
        }
    }
}


