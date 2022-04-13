using System.Collections.Generic;
using Core.Systems;
using JetBrains.Annotations;
using UnityEngine;

namespace Core.Game
{
    [CreateAssetMenu(fileName = "", menuName = "Game/WorldData", order = 1)]
    public class WorldDataItem : ScriptableObject
    {
        private static WorldDataItem Instance;
     
        
        
        [System.Serializable]
        public class Data
        {
            [SerializeField]
            private List<IconName> types = new List<IconName>();
            
            public Sprite IconByID(int id)
            {
                return types[id].icon;
            }
            public int NameToID(string name)
            {
                return types.FindIndex(x => x.typeName.ToLower().Trim() == name.ToLower().Trim());
            }
            public IconName ByID(int id)
            {
                return types[id];
            }

            public int Count => types.Count;
            
            public void Add([NotNull] Sprite icon, [NotNull] string name)
            {
                types.Add(new IconName(icon, name));
            }

            public string NameByID(int fractionID)
            {
                return types[fractionID].typeName;
            }
        }
        [System.Serializable]
        public class IconName
        {    
            public string typeName;
            public Sprite icon;

            public IconName([NotNull] Sprite icon, [NotNull] string name)
            {
                this.typeName = name;
                this.icon = icon;
            }
        }
        

        public Data quests = new Data();
        public Data fractions = new Data();
        public static Data Quests => GetData().quests;
        public static Data Fractions => GetData().fractions;
        
        
        public static WorldDataItem GetData()
        {
            if (Instance == null)
            {
                Instance = Resources.LoadAll<WorldDataItem>("")[0];
            }
            return Instance;
        }
        
        
        
        
        
        
        
        
    }
}
