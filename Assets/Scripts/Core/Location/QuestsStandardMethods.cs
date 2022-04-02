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

            var cost = 0;
            for (int i = 0; i < quest.toTransfer.Count; i++)
            {
                cost += (int) quest.toTransfer[i].amount.value * rnd.Next(15, 25);
            }

            quest.reward.Init(quest.questID, cost);
        }



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
    }
}
