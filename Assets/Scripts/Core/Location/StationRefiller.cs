using System;
using System.Collections.Generic;
using Core.Game;
using Core.PlayerScripts;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    public class StationRefiller : Singleton<StationRefiller>
    {
        [Serializable]
        public class Refiller
        {
            public enum RefillType
            {
                Fuel = 0,
                Curpus = 1
            }

            public RefillType refillType;
            public ValueLimit valueLimit;
        }

        [SerializeField] List<Refiller> refillers;

        public void InitRefiller(int seed)
        {
            Single(this);
            var rnd = new Random(seed);
            for (int i = 0; i < refillers.Count; i++)
            {
                refillers[i].valueLimit.SetValue(NextFloat(rnd, refillers[i].valueLimit.Min, refillers[i].valueLimit.Max));
            }
        }

        public float GetRefillerValue(Refiller.RefillType type)
        {
            return refillers.Find(x => x.refillType == type).valueLimit.Value;
        }
    
        public float NextFloat(Random rnd, float min, float max)
        {
            return min + (max - min) * (float)rnd.NextDouble();
        }

        public void Fill(Refiller.RefillType type)
        {
            var cost = 0f;
            switch (type)
            {
                case Refiller.RefillType.Fuel:
                    cost = GetRefillerValue(Refiller.RefillType.Fuel);
                    ApplyType(PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(ItemShip.ShipValuesTypes.Fuel), cost);
                    break;
                case Refiller.RefillType.Curpus:
                    cost = GetRefillerValue(Refiller.RefillType.Curpus);
                    ApplyType(PlayerDataManager.Instance.WorldHandler.ShipPlayer.Ship().GetValue(ItemShip.ShipValuesTypes.Health), cost);
                    break;
            }
        }

        public void ApplyType(ShipClaped data, float cost)
        {
            while (PlayerDataManager.Instance.WorldHandler.ShipPlayer.Cargo.GetCredits() > 0 && data.value < data.max)
            {
                if(PlayerDataManager.Instance.WorldHandler.ShipPlayer.Cargo.RemoveCredits(cost))
                {
                    data.value++;
                }
                else
                {
                    break;
                }
            }
            data.Clamp();
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.Cargo.UpdateInventory();
        }
    }
}
