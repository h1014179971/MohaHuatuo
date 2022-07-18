using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using libx;

namespace Space
{
    public class MapCtrl : MonoSingleton<MapCtrl>
    {
        private MapModel _mapModel;
        private Map _map;
        public Vector3 CenterPos { get; set; }
        public void Init(Map map)
        {
            _map = map;
            CreateMap(_map);
        }
        private void CreateMap(Map map)
        {
            Debug.Log($"CreateMap£º{Tags.map}");
            GameObject lastMapObj = GameObject.FindGameObjectWithTag(Tags.map);
            if (lastMapObj != null)
                Destroy(lastMapObj);
            GameObject prefab = AssetLoader.Load<GameObject>(map.prefabName + ".prefab");
            GameObject mapObj = GameObject.Instantiate<GameObject>(prefab);
            Debug.Log($"CreateMap1111");
            _mapModel = mapObj.GetOrAddComponent<MapModel>();
            _mapModel.Init(map);
            CenterPos = mapObj.transform.position;


        }
    }
}

