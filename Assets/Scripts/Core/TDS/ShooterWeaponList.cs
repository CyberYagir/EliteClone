using System.Collections.Generic;
using Core.Game;
using UnityEngine;

namespace Core.TDS
{
    public class ShooterWeaponList : MonoBehaviour
    {
        [System.Serializable]
        public class Weapon
        {
            public GameObject mesh;
            public Item item;
            public Transform bulletPoint;
            public Object script;
            public string weaponScript;
            public TDSWeaponOptions options;
        }

        public List<Weapon> weapons;
    }
}