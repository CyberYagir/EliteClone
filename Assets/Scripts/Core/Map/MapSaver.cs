using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Core.Map;
using Newtonsoft.Json;
using UnityEngine;

namespace Core
{
    public class MapSaver : MonoBehaviour
    {
        public static MapData data; 
        
        [System.Serializable]
        public class MapData
        {
            public string start, end;
        }

        private void Start()
        {
            Load();
        }

        public void Load()
        {
            if (data == null && File.Exists(PlayerDataManager.Instance.FSHandler.MapFile))
            {
                data = JsonConvert.DeserializeObject<MapData>(File.ReadAllText(PlayerDataManager.Instance.FSHandler.MapFile));
            }
            if (data != null && data.start != "" && data.end != "")
            {
                MapPathfinder.Instance.SetStartPath(data.start);
                MapPathfinder.Instance.SetEndPath(data.end);
                MapPathfinder.Instance.skipFrames = false;
                MapPathfinder.Instance.FindPath();
                MapPathfinder.Instance.skipFrames = true;
            }
        }
        public void Save()
        {
            data = new MapData() {start = MapPathfinder.Instance.start, end = MapPathfinder.Instance.end};
            File.WriteAllText(PlayerDataManager.Instance.FSHandler.MapFile, JsonConvert.SerializeObject(data));
        }
    }
}
