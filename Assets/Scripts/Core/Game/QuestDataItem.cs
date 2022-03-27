using System.Collections;
using System.Collections.Generic;
using Quests;
using UnityEngine;

[CreateAssetMenu(fileName = "", menuName = "Game/QuestsData", order = 1)]
public class QuestDataItem : ScriptableObject
{
    private static QuestDataItem Instance;
    public static QuestDataItem GetData()
    {
        if (Instance == null)
        {
            Instance = Resources.LoadAll<QuestDataItem>("")[0];
        }
        return Instance;
    }
    
    
    [System.Serializable]
    public class MineTypes
    {
        public Quest.QuestType type;
        public Sprite icon;
    }
    public List<MineTypes> mineType;
}
