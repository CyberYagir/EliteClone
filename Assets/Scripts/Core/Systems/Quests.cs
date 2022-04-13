using System;
using System.Collections.Generic;
using Core.Game;
using Core.Location;
using Core.Player;
using Random = System.Random;

namespace Core.Systems
{
    [System.Serializable]
    public class QuestPath
    {
        public string solarName = "", targetName = "";
        public QuestPath prevPath, nextPath;
        public QuestPath()
        {
            
        }
        
        public bool isFirst
        {
            get
            {
                return prevPath == null;
            }
        }

        public bool isLast
        {
            get
            {
                return nextPath == null;
            }
        }
    }

    public class Reward
    {
        public enum RewardType
        {
            Money, Item, Items
        }

        public RewardType type;
        public List<Item> rewardItems = new List<Item>();

        public void Init(int questID, float settedMoney = -1)
        {
            var rnd = new Random(questID);
            type = (RewardType) rnd.Next(0, Enum.GetNames(typeof(RewardType)).Length);
            
            if (type == RewardType.Money)
            {
                var money = ItemsManager.GetCredits().Clone();
                money.amount.SetValue(rnd.Next(5000, 20000));
                rewardItems.Add(money);
            }
            else
            {
                int count = 1;
                if (type == RewardType.Items)
                {
                    count += rnd.Next(0, 2);
                }
                for (int i = 0; i < count; i++)
                {
                    rewardItems.Add(ItemsManager.GetRewardItem(rnd));
                }
                var money = ItemsManager.GetCredits().Clone();
                if (settedMoney == -1)
                {
                    money.amount.SetValue(rnd.Next(1000, 3000));
                }
                else
                {
                    money.amount.SetValue(settedMoney);
                }

                rewardItems.Add(money);
            }
        }
    }


    [System.Serializable]
    public class Character
    {
        public string firstName;
        public string lastName;
        public int characterID;
        
        
        public int fraction;

        public Character(System.Random rnd)
        {
            Init(rnd);
        }

        public Character()
        {
            
        }

        public void Reset()
        {
            firstName = null;
            lastName = null;
        }
        public void Init(System.Random rnd)
        {
            characterID = rnd.Next(-9999999, 9999999);
            NamesHolder.Init();
            firstName = NamesHolder.ToUpperFist(NamesHolder.GetFirstName(rnd));
            lastName = NamesHolder.ToUpperFist(NamesHolder.GetLastName(rnd));
            fraction = rnd.Next(0, WorldDataItem.Fractions.Count);
        }
    }
}


