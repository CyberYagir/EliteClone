using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class StationRefiller : MonoBehaviour
{
    [Serializable]
    public class Refiller
    {
        public enum RefillType
        {
            Fuel,
            Curpus
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
}
