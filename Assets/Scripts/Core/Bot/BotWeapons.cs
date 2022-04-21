using System.Collections;
using System.Collections.Generic;
using Core.Game;
using UnityEngine;

namespace Core.Bot
{
    public class BotWeapons : MonoBehaviour
    {
        [SerializeField]
        private List<Item> weapons = new List<Item>();

        public Item GetWeapon(int seed)
        {
            var rnd = new System.Random(seed);
            
            return weapons[rnd.Next(0, weapons.Count)];
        }
    }
}
