using System;
using System.Collections;
using System.Collections.Generic;
using Core.Bot;
using Core.Systems;
using DG.Tweening;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    public class WorldOrbitalStationPoints : MonoBehaviour
    {
        [SerializeField] private List<LandPoint> landPoints;
        [SerializeField] private GameObject botPrefab;
        [SerializeField] private List<Transform> botPoints;
        private WorldOrbitalStation worldOrbitalStation;
        private SolarSystemShips solarSystemShips;
        private WorldDataHandler worldHandler;

        public void Init(WorldOrbitalStation worldOrbitalStation, SolarSystemShips solarSystemShips)
        {
            this.worldOrbitalStation = worldOrbitalStation;
            worldHandler = PlayerDataManager.Instance.WorldHandler;
            this.solarSystemShips = solarSystemShips;
            Random rnd = new Random(worldOrbitalStation.GetUniqSeed() + SaveLoadData.GetCurrentSaveSeed());
            for (int i = 0; i < landPoints.Count; i++)
            {
                if (rnd.Next(0, 100) <= 30)
                { 
                    if (!landPoints[i].isFilled)
                    {
                        var bot = Instantiate(botPrefab, landPoints[i].point.transform.position, landPoints[i].point.transform.rotation);
                        bot.transform.parent = transform;
                        var builder = bot.GetComponent<BotBuilder>();
                    
                        builder.GetVisual().SetLights(false);
                        builder.GetShield().isActive = true;
                        builder.AddContact(false);
                        builder.SetHuman(SolarSystemShipsStaticBuilder.GenerateHuman(rnd, 0, 0));
                        builder.InitBot(worldHandler, solarSystemShips, rnd);
                        builder.GetVisual().SetVisual(rnd);
                        builder.SetBehaviour(BotBuilder.BotState.Land);
                        builder.SetLandPoint(landPoints[i]);
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

        public ref List<LandPoint> GetLandPoint()
        {
            return ref landPoints;
        }

        public IEnumerator SpawnNewBot()
        {
            bool isEnded = false;
            int trys = 0;
            Random rnd = new Random(worldOrbitalStation.GetUniqSeed() + SaveLoadData.GetCurrentSaveSeed() + DateTime.Now.Hour + DateTime.Now.Second);
            int id = 0;
            while (!isEnded)
            {
                var landPoint = landPoints[UnityEngine.Random.Range(0, landPoints.Count)];
                id++;
                if (!landPoint.isFilled)
                {
                    foreach (var spawns in botPoints)
                    {
                        id++;

                        if (Physics.Raycast(spawns.position, landPoint.transform.GetChild(0).position - spawns.position, out RaycastHit hit))
                        {
                            Debug.DrawRay(spawns.position, landPoint.transform.GetChild(0).position - spawns.position, Color.cyan, 999999);
                            if (hit.transform == landPoint.transform)
                            {
                                var bot = Instantiate(botPrefab, spawns.position, spawns.rotation);
                                var builder = bot.GetComponent<BotBuilder>();
                                builder.PlayWarp();
                                builder.GetShield().isActive = true;
                                landPoint.isFilled = true;
                                isEnded = true;
                                builder.InitBot(worldHandler, solarSystemShips, rnd);
                                builder.SetHuman(SolarSystemShipsStaticBuilder.GenerateHuman(rnd, id, rnd.Next(0, 999)));
                                builder.GetVisual().SetVisual(rnd);
                                builder.AddContact(true);
                                builder.SetLandPoint(landPoint);
                                bot.transform.DOMove(landPoint.point.position, 5);
                                bot.transform.DORotate(landPoint.point.eulerAngles, 5);
                                builder.SetBehaviour(BotBuilder.BotState.Land);
                            
                                yield return new WaitForSeconds(6);
                                builder.GetVisual().SetLights(false);
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
}
