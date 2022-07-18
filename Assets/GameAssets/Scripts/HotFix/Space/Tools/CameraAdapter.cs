using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Space
{
    public class CameraAdapter : MonoBehaviour
    {
        private Camera _camera;
        void Start()
        {
            _camera = GetComponent<Camera>();
            float rate = Screen.height * 1.0f / Screen.width;
            float fixRate = FixScreen.height / FixScreen.width;
            if (rate > fixRate)
            {
                float size = rate / fixRate * FixScreen.idleCameraSize;
                _camera.orthographicSize = size;
            }

        }
    }
}

