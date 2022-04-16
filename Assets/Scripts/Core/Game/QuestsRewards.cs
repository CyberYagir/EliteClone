using System.Collections.Generic;
using UnityEngine;

namespace Core.Game
{
    [CreateAssetMenu(fileName = "", menuName = "Game/Rewards", order = 1)]
    public class QuestsRewards : ScriptableObject
    {
        public Item creditItem;
        public List<Item> canBeRewarded;
        public List<Item> canBeTransfered;
        public List<Item> canBeMineral;
    }
}
