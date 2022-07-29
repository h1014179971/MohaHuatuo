using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Foundation;

namespace MHSpace
{
    [System.Serializable]
    public class Player 
    {
        public int pveLv = 1;
        public Long2 money;
        public Dictionary<PowerType, int> powerDict = new Dictionary<PowerType, int>();
    }
}

