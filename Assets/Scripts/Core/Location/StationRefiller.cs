using System;
using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using Random = System.Random;

public class StationRefiller : MonoBehaviour
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

    public static StationRefiller Instance;
    [SerializeField] List<Refiller> refillers;

    public void InitRefiller(int seed)
    {
        Instance = this;
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
    
    public float NextFloat(System.Random rnd, float min, float max)
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
                ApplyType(Player.inst.Ship().GetValue(ItemShip.ShipValuesTypes.Fuel), cost);
                break;
            case Refiller.RefillType.Curpus:
                cost = GetRefillerValue(Refiller.RefillType.Curpus);
                ApplyType(Player.inst.Ship().GetValue(ItemShip.ShipValuesTypes.Health), cost);
                break;
        }
    }

    public void ApplyType(ShipClaped data, float cost)
    {
        while (Player.inst.cargo.GetCredits() > 0 && data.value < data.max)
        {
            if(Player.inst.cargo.RemoveCredits(cost))
            {
                data.value++;
            }
        }
        data.Clamp();
        Player.inst.cargo.UpdateInventory();
    }
}
