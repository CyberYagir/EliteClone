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
    
    
    public void Init()
    {
        var rnd = new Random((int) transform.position.sqrMagnitude);
        var beltData = PlayerDataManager.CurrentSolarSystem.belts.Find(x => x.name == transform.name).beltData;
        List<GameObject> meteors = new List<GameObject>();
        for (int i = 0; i < beltData.meteorsCount; i++)
        {
            var met = Instantiate(meteor.gameObject).GetComponent<Meteor>();
            met.Init(rnd);
            met.transform.position = new Vector3(rnd.Next(-maxRadius, maxRadius), rnd.Next(-200, 200), rnd.Next(-maxRadius, maxRadius));
            met.transform.parent = transform;
            met.gameObject.isStatic = true;
            meteors.Add(met.gameObject);
        }

        transform.gameObject.isStatic = true;
        StaticBatchingUtility.Combine(meteors.ToArray(), gameObject);
    }
}
