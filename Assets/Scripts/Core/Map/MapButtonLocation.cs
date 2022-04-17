using System.Collections;
using System.Collections.Generic;
using Core.Garage;
using Core.Map;
using UnityEngine;

namespace Core.Map
{
    public class MapButtonLocation : MonoBehaviour
    {
        [SerializeField] private MapGenerator generator;
        [SerializeField] private FaderMultiScenes waiter;
        public void Init()
        {
            var saves = generator.GetSaves();
            var key = saves.GetKey("MapActive");
            if (key != null)
            {
                var scene = int.Parse(key.ToString());
                waiter.SetScene(scene);
            }
        }
    }
}
