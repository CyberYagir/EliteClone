using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Game
{
    [CreateAssetMenu(fileName = "", menuName = "Game/Weapon Options", order = 1)]
    public class WeaponOptionsItem : ScriptableObject
    {
        [Serializable]
        public class LaserOptions
        {
            public AnimationCurve width;
            public Material materal;
        }
        
        [Serializable]
        public class NameToObject
        {
            public string name;
            public Object obj;
        }

        public LaserOptions laserOptions;
        public float maxDistance;
        public GameObject attackParticles;
        public GameObject attackDecal;
        public List<NameToObject> objects;

        public Object GetObject(string objectName)
        {
            return objects.Find(x => x.name == objectName)?.obj;
        }
    }
}
