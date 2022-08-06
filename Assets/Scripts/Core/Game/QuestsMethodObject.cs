using System.Collections;
using System.Collections.Generic;
using Core.Location;
using UnityEditor;
using UnityEngine;

namespace Core.Quests
{
    [CreateAssetMenu(fileName = "", menuName = "Game/Quests Methods", order = 1)]
    public class QuestsMethodObject : ScriptableObject
    {
        [SerializeField] private List<WorldStationQuests.QuestFunction> methods = new List<WorldStationQuests.QuestFunction>();

        public List<WorldStationQuests.QuestFunction> Methods => methods;
        
        public WorldStationQuests.QuestFunction GetEventByID(int id)
        {
            return Methods.Find(x => x.questType == id);
        }
    }
}
