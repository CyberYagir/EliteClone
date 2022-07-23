using System.Collections;
using System.Collections.Generic;
using Core.Demo;
using Core.TDS.UI;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class CommunistsQuests : MonoBehaviour
    {
        [SerializeField] private UIQuestVisual quests;
        public void SetToBarmanQuests()
        {
            quests.SetQuest(new TDSQuest()
            {
                Text = "Ask the barman about the communist."
            });
        }

        public void GetInformationFromComputers()
        {
            quests.SetQuest(new TDSQuest()
            {
                Text = "Kill guards and hack the terminals."
            });
        }

        public void OutForStation()
        {
            quests.SetQuest(new TDSQuest()
            {
                Text = "Leave the station."
            });
        }
    }
}
