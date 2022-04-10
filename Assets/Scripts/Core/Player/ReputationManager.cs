using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Player
{
    public class ReputationManager : MonoBehaviour
    {
        public static List<string> fractions = new List<string>()
        {
            "Pirates",
            "Libertarians",
            "Communists",
            "Anarchists",
            "OCG"
        };

        public static int NameToID(string _name)
        {
            return fractions.FindIndex(x => x == _name);
        }
        public static string IDToName(int id)
        {
            return fractions[id];
        }
    }
}
