using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

public class WorldOrbitalStationPoints : MonoBehaviour
{
    [SerializeField] private List<LandPoint> landPoints;
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private List<Transform> botPoints;

    private void Awake()
    {
        Random rnd = new Random(DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour);

        for (int i = 0; i < landPoints.Count; i++)
        {
            if (rnd.Next(0, 100) <= 30)
            { 
                if (!landPoints[i].isFilled)
                {
                    var bot = Instantiate(botPrefab, landPoints[i].point.transform.position, landPoints[i].point.transform.rotation);
                    bot.transform.parent = transform;
                    bot.GetComponent<BotVisual>().SetVisual(rnd);
                    bot.GetComponent<BotVisual>().SetLights(false);
                    Destroy(bot.GetComponent<WorldSpaceObject>());
                    landPoints[i].isFilled = true;
                }
            }
        }

        StartCoroutine(BotWaiter());
    }

    IEnumerator BotWaiter()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(10, 50));
            yield return StartCoroutine(SpawnNewBot());
        }
    }
    
    public IEnumerator SpawnNewBot()
    {
        bool isEnded = false;
        int trys = 0;
        while (!isEnded)
        {
            var landPoint = landPoints[UnityEngine.Random.Range(0, landPoints.Count)];

            if (!landPoint.isFilled)
            {
                foreach (var spawns in botPoints)
                {
                    if (Physics.Raycast(spawns.position, landPoint.transform.position - spawns.position, out RaycastHit hit))
                    {
                        if (hit.transform == landPoint.transform)
                        {
                            var bot = Instantiate(botPrefab, spawns.position, spawns.rotation);
                            bot.transform.DOMove(landPoint.point.position, 5);
                            bot.transform.DORotate(landPoint.point.eulerAngles, 5);
                            bot.GetComponent<BotVisual>().SetVisual(new Random(DateTime.Now.Second));
                            landPoint.isFilled = true;
                            isEnded = true;

                            yield return new WaitForSeconds(6);
                            bot.GetComponent<BotVisual>().SetLights(false);

                            break;
                        }

                        yield return null;
                    }

                    yield return null;
                }
            }

            yield return null;
            trys++;
            if (trys > 40) break;
        }
    }
}