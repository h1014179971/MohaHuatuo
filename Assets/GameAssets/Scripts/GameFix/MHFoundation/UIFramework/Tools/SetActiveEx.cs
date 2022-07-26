using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexSnake
{
    public class SetActiveEx : MonoBehaviour
    {
        private Vector3 _startPos;
        private Vector3 _endPos;

        private void Awake()
        {
            _startPos = transform.localPosition;
            _endPos = new Vector3(10000, 10000, 0);
        }

        public void SetActive(bool active)
        {
            if (active)
                transform.localPosition = _startPos;
            else
                transform.localPosition = _endPos;
        }

    }
}

