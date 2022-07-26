using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Foundation;
using libx;

namespace MHSpace
{
    public class MapMgr : Singleton<MapMgr>
    {
        private List<Map> _allMaps = new List<Map>();
        public List<Map> AllMaps { get { return _allMaps; } }
        private int _maxMapId;
        private int _minMapId;
        public override void Init()
        {
            Debug.Log($"MapMgr Init");
            ReadFile();
        }

        private void ReadFile()
        {
            TextAsset textAsset = AssetLoader.Load<TextAsset>(Files.map);
            string jsonStr = textAsset.text;
            List<Map> maps = FullSerializerAPI.Deserialize(typeof(List<Map>), jsonStr) as List<Map>;
            _allMaps = maps;
            _maxMapId = _allMaps.Max(t => t.id);
            _minMapId = _allMaps.Min(t => t.id); 
            Debug.Log($"ReadFile333:{_allMaps.Count}");
        }

        public int GetMaxId()
        {
            return _maxMapId;
        }

        public int GetMinId()
        {
            return _minMapId;
        }

        public Map GetMapById(int mapId)
        {
            Map map = null;
            map = _allMaps.Find(item => item.id == mapId);
            if (map == null)
            {
                LogUtility.LogError($"ц╩сп{mapId}id");
            }
            return map;
        }
    }
}

