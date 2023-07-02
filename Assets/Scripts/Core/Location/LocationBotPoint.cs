using System.Collections.Generic;
using Core.Bot;
using UnityEngine;
using Random = System.Random;

namespace Core.Location
{
    public enum LocationBotType
    {
        Once, Convoy, OCG
    }
    public class LocationBotPoint : MonoBehaviour
    {
    
        [SerializeField] private int uniqID;
        [SerializeField] private LocationBotType type;

        private SolarSystemShips.HumanShip currentPerson;
        private SolarSystemShips solarSystemShips;
        private WorldDataHandler worldDataHandler;

        public void Init(SolarSystemShips solarSystemShips, WorldDataHandler worldDataHandler)
        {
            this.worldDataHandler = worldDataHandler;
            this.solarSystemShips = solarSystemShips;
            
            
            var data = LocationGenerator.CurrentSave.otherKeys;
            if ((string)data["tag"] == "ships")
            {
                uniqID = int.Parse(data["uniqID"].ToString());
                type = (LocationBotType)int.Parse(data["tag-type"].ToString());
                currentPerson = solarSystemShips.GetShips().Find(x => x.uniqID == uniqID);

                if (currentPerson == null || solarSystemShips.IsDead(currentPerson.uniqID))
                {
                    return;
                }
            }
            else
            {
                return;
            }

            CreateBots();
        }

        public void CreateBots()
        {
            var mainBot = SpawnMainBot();
            BotBuilder mainBuilder = null;
            if (mainBot)
            {
                mainBuilder = mainBot.GetComponent<BotBuilder>();
            }
            var rnd = new Random(uniqID);
            if (mainBot != null)
            {
                if (type == LocationBotType.Convoy)
                {
                    for (int i = 0; i < rnd.Next(2, 5); i++)
                    {
                        var bot = SpawnRandomBot(2, rnd).GetComponent<BotBuilder>();
                        bot.GetDamager().OnDamaged += mainBuilder.AttackPlayer;
                    }
                }

                if (type == LocationBotType.OCG)
                {
                    List<BotBuilder> bots = new List<BotBuilder>();
                    bots.Add(mainBuilder);
                    for (int i = 0; i < rnd.Next(1, 8); i++)
                    {
                        var bot = SpawnRandomBot(rnd.Next(0, 2), rnd).GetComponent<BotBuilder>();
                        bots.Add(bot);
                    }

                    for (int i = 0; i < bots.Count; i++)
                    {
                        for (int j = 0; j < bots.Count; j++)
                        {
                            bots[i].GetDamager().OnDamaged += bots[j].AttackPlayer;
                        }
                    }
                }
            }
        }

        public GameObject SpawnRandomBot(int visuals, Random rnd)
        {
            var pos = new Vector3(rnd.Next(-300, 300), rnd.Next(-300, 300), rnd.Next(-300, 300));
            var bot = solarSystemShips.CreateBot(null, BotBuilder.BotState.Stationary);
            bot.transform.position = pos;
            bot.InitBot(worldDataHandler,rnd);
            bot.GetVisual().SetVisual(visuals);
            bot.SetHuman(SolarSystemShipsStaticBuilder.GenerateHuman(rnd, 0, 0));
            bot.SetName();
            return bot.gameObject;
        }
    
        public GameObject SpawnMainBot()
        {
            var rnd = new Random(uniqID);
            var pos = new Vector3(rnd.Next(-100, 100), rnd.Next(-100, 100), rnd.Next(-100, 100));
            var bot = solarSystemShips.CreateBot(currentPerson, BotBuilder.BotState.Stationary);
            bot.transform.position = pos;

            return bot.gameObject;
        }
    }
}