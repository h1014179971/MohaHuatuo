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
            Debug.Log($"CreateMap：{Tags.map}");
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
        /// <summary>
        /// 地图上随机一点
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public Vector3 RandomPos(out float angle, float radius = 7)
        {
            //float radius = 7;
            float x = Random.Range(-radius, radius);
            float y = Mathf.Sqrt(radius * radius - x * x);
            if (Random.Range(0, 2) == 0)
                y = -y;
            Vector3 pos = new Vector3(x, y, 0);
            angle = Quaternion.FromToRotation(Vector3.up, pos).eulerAngles.z;
            pos += MapCtrl.Instance.CenterPos;
            return pos;
        }
    }
}

