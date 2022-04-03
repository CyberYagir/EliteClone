using System.Collections;
using System.Collections.Generic;
using Core.Player;
using Core.Systems;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    /// <summary>
    /// Orbital base call events by name
    /// </summary>
    public class QuestsStandardMethods : MonoBehaviour
    {

        #region Inits

        
        public void InitTransfer(Quest quest)
        {
            System.Random rnd = new Random(quest.questID);
            int transfersCount = rnd.Next(1, 3);
            for (int i = 0; i < transfersCount; i++)
            {
                quest.toTransfer.Add(ItemsManager.GetTransferedItem(rnd));
            }

            quest.pathToTarget = quest.GetPath(rnd, quest.appliedStation, quest.appliedSolar);
            quest.reward.Init(quest.questID);
        }

        public void InitMine(Quest quest)
        {
            System.Random rnd = new Random(quest.questID);
            int transfersCount = rnd.Next(1, 3);
            for (int i = 0; i < transfersCount; i++)
            {
                var mineral = ItemsManager.GetMineralItem(rnd);
                mineral.amount.SetValue(mineral.amount.Max);
                quest.toTransfer.Add(mineral);
            }

            quest.pathToTarget = new QuestPath() {solarName = quest.appliedSolar, targetName = quest.appliedStation};
            quest.keyValues.Add("NoAddTransfer", true);
            var cost = 0;
            for (int i = 0; i < quest.toTransfer.Count; i++)
            {
                cost += (int) quest.toTransfer[i].amount.value * rnd.Next(15, 25);
            }

            quest.reward.Init(quest.questID, cost);
        }

        public void InitKill(Quest quest)
        {
            System.Random rnd = new Random(quest.questID);
            quest.pathToTarget = new QuestPath() {solarName = quest.appliedSolar, targetName = quest.appliedStation};
            var ships = SolarSystemShips.GetShips(PlayerDataManager.CurrentSolarSystem).FindAll(x => x.fraction != quest.quester.fraction);
            if (ships.Count != 0)
            {
                var id = rnd.Next(0, ships.Count);
                var target = ships[id];
                quest.keyValues.Add("BotTarget", target.uniqID);
                var text = "Your goal is to destroy a man named ";
                text += "<color=orange>" + NamesHolder.ToUpperFist(ships[id].firstName) + " " + NamesHolder.ToUpperFist(ships[id].lastName) + "</color>.\n";
                text += "Vehicle: " + ItemsManager.GetShipItem(ships[id].shipID)?.shipName + "\n";
                text += "Fraction: " + ships[id].fraction;
                quest.keyValues.Add("Text", text);
                quest.reward.Init(quest.questID, 5000);
            }
            else
            {
                quest.questState = Quest.QuestComplited.BrokeQuest;
            }
        }
        

        #endregion


        #region IsComplete

        
        public void IsMinerTransferIsComplete(Quest quest)
        {
            if (quest.IsTypeQuest("Transfer") || quest.IsTypeQuest("Mine"))
            {
                bool allItemInInventory = Player.Player.inst.cargo.ContainItems(quest.toTransfer);
                var last = quest.GetLastQuestPath();
                if (allItemInInventory)
                {
                    if (last.solarName == PlayerDataManager.CurrentSolarSystem.name)
                    {
                        if (WorldOrbitalStation.Instance.transform.name == last.targetName)
                        {
                            quest.questState = Quest.QuestComplited.Complited;
                        }
                    }
                }
            }
        }

        public void IsKillIsComplete(Quest quest)
        {
            if (quest.IsTypeQuest("Kill"))
            {
                SolarSystemShips.LoadDeads();
                foreach (var deads in SolarSystemShips.deadList)
                {
                    foreach (var ship in deads.Value)
                    {
                        if (ship.uniqID == (int)quest.keyValues["BotTarget"])
                        {
                            quest.questState = Quest.QuestComplited.Complited;
                            return;
                        }
                    }
                }
            }
        }

        #endregion


        #region Display

        public void MineButtonDisplay(Quest quest)
        {
            if (quest.IsTypeQuest("Mine"))
            {
                if (WorldOrbitalStation.Instance.transform.name == quest.appliedStation)
                {
                    if (Player.Player.inst.cargo.ContainItems(quest.toTransfer) && quest.questState == Quest.QuestComplited.Complited)
                    {
                        quest.buttonText = "Finish";
                    }
                    else
                    {
                        quest.buttonText = "Cancel";
                    }
                }
            }
        }
        
        public void TransferButtonDisplay(Quest quest)
        {
            if (quest.IsTypeQuest("Transfer"))
            {
                if (WorldOrbitalStation.Instance.transform.name == quest.GetLastQuestPath().targetName)
                {
                    quest.buttonText = "Items to transfer missing";
                }
                else if (WorldOrbitalStation.Instance.transform.name == quest.appliedStation)
                {
                    if (Player.Player.inst.cargo.ContainItems(quest.toTransfer))
                    {
                        quest.buttonText = "Cancel";
                    }
                    else
                    {
                        quest.buttonText = "Items to transfer missing [cant cancel]";
                    }
                }
            }
        }

        public void KillButtonDisplay(Quest quest)
        {
            if (quest.IsTypeQuest("Kill"))
            {
                if (quest.questState == Quest.QuestComplited.Complited)
                {
                    quest.buttonText = "Finish";
                }
                else
                {
                    quest.buttonText = "Cancel";
                }
            }

        }
        
        #endregion


        #region EndQuest

        public void IsMinerTransferIsCompleted(Quest quest)
        {
            if (quest.IsTypeQuest("Transfer") || quest.IsTypeQuest("Mine"))
            {
                if (Player.Player.inst.cargo.ContainItems(quest.toTransfer))
                {
                    Player.Player.inst.cargo.RemoveItems(quest.toTransfer);
                    if (Player.Player.inst.cargo.AddItems(quest.reward.rewardItems))
                    {
                        if (AppliedQuests.Instance.FinishQuest(quest.questID))
                        {
                            quest.questState = Quest.QuestComplited.Rewarded;
                        }
                    }
                    else
                    {
                        Player.Player.inst.cargo.AddItems(quest.toTransfer);
                    }
                }
            }
        }
        
        public void IsKillCompleted(Quest quest)
        {
            if (quest.IsTypeQuest("Kill"))
            { 
                if (quest.questState == Quest.QuestComplited.Complited)
                {
                    if (Player.Player.inst.cargo.AddItems(quest.reward.rewardItems))
                    {
                        if (AppliedQuests.Instance.FinishQuest(quest.questID))
                        {
                            quest.questState = Quest.QuestComplited.Rewarded;
                        }
                    }
                }
            }
        }

        #endregion

    }
}
