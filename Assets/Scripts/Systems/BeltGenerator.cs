using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class BeltGenerator : MonoBehaviour
{
    [SerializeField] private GameObject meteor;
    [SerializeField] private int maxRadius = 10000;
    [SerializeField] private int seed;
    [SerializeField] private List<Item> minerals, mineMinerals;
    [SerializeField] private int maxMineralsCount;
    

    public void Init()
    {
        seed = WorldOrbitalStation.CalcSeed(transform.name, LocationGenerator.CurrentSave.GetSystemCode());
        var rnd = new Random(seed);
        var mineralsCount = rnd.Next(1, maxMineralsCount);

        for (int i = 0; i < mineralsCount; i++)
        {
            var item = minerals[rnd.Next(0, minerals.Count)];
            mineMinerals.Add(item);
        }
        
        var beltData = PlayerDataManager.CurrentSolarSystem.belts.Find(x => x.name == transform.name).beltData;
        List<GameObject> meteors = new List<GameObject>();
        for (int i = 0; i < beltData.meteorsCount; i++)
        {
            var met = Instantiate(meteor.gameObject).GetComponent<Meteor>();
            met.Init(rnd, mineMinerals[rnd.Next(0, mineMinerals.Count)]);
            met.transform.position = new Vector3(rnd.Next(-maxRadius, maxRadius), rnd.Next(-200, 200), rnd.Next(-maxRadius, maxRadius));
            met.transform.parent = transform;
            //met.gameObject.isStatic = true;
            meteors.Add(met.gameObject);
        }

        //transform.gameObject.isStatic = true;
        //StaticBatchingUtility.Combine(meteors.ToArray(), gameObject);
    }
}
