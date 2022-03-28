using System.Collections.Generic;
using Core.Game;
using UnityEngine;

namespace Core.Garage
{
    public class TradeManager : MonoBehaviour
    {
        [System.Serializable]
        public class TradeOffer
        {
            public Item item;
            public float minCost = -1 , maxCost = -1;
            public AnimationCurve costLevel;
        }
        [System.Serializable]
        public class Offer
        {
            public Item item;
            public float cost;
        }
    
        public List<TradeOffer> staticOffers;
        public List<Offer> offers = new List<Offer>();
        public Event OnUpdateOffers = new Event();
        public void Start()
        {
            UpdateList();
        }

        public void UpdateList()
        {
            var rnd = new System.Random(GarageDataCollect.Instance.stationSeed);
            for (int i = 0; i < staticOffers.Count; i++)
            {
                var ev = staticOffers[i].costLevel.Evaluate((float) rnd.NextDouble());
                var delta = staticOffers[i].maxCost - staticOffers[i].minCost;
                var percent = delta * ev;
            
                offers.Add(new Offer(){ item = staticOffers[i].item, cost =staticOffers[i].minCost + percent});
            }
            OnUpdateOffers.Run();
        }

        public float GetItemCost(string idName)
        {
            var findItem = offers.Find(x => x.item.id.idname == idName);
            if (findItem != null)
            {
                return findItem.cost;
            }

            return 0;
        }
    }
}
