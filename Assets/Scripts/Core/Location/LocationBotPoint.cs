using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public enum LocationBotType
{
    Once, Convoy, OCG
}
public class LocationBotPoint : MonoBehaviour
{
    
    [SerializeField] private int uniqID;
    [SerializeField] private LocationBotType type;
    [SerializeField] private BotBuilder botPrefab;

    private SolarSystemShips.HumanShip currentPerson;
    public void Init()
    {
        var data = LocationGenerator.CurrentSave.otherKeys;
        if ((string)data["tag"] == "ships")
        {
            uniqID = int.Parse(data["uniqID"].ToString());
            type = (LocationBotType)int.Parse(data["tag-type"].ToString());
            currentPerson = SolarSystemShips.Instance.GetShips().Find(x => x.uniqID == uniqID);

            if (currentPerson == null || SolarSystemShips.Instance.IsDead(currentPerson.uniqID))
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
        var rnd = new Random(uniqID);
        if (mainBot != null)
        {
            if (type == LocationBotType.Convoy)
            {
                for (int i = 0; i < rnd.Next(2, 5); i++)
                {
                    SpawnRandomBot(2, rnd);
                }
            }

            if (type == LocationBotType.OCG)
            {
                for (int i = 0; i < rnd.Next(1, 8); i++)
                {
                    SpawnRandomBot(rnd.Next(0, 2), rnd);
                }
            }
        }
    }

    public GameObject SpawnRandomBot(int visuals, Random rnd)
    {
        var pos = new Vector3(rnd.Next(-300, 300), rnd.Next(-300, 300), rnd.Next(-300, 300));
        var bot = SolarSystemShips.Instance.CreateBot(null, BotBuilder.BotState.Stationary);
        bot.transform.position = pos;
        bot.InitBot(rnd);
        bot.GetVisual().SetVisual(visuals);
        bot.SetName();

        return bot.gameObject;
    }
    
    public GameObject SpawnMainBot()
    {
        var rnd = new Random(uniqID);
        var pos = new Vector3(rnd.Next(-100, 100), rnd.Next(-100, 100), rnd.Next(-100, 100));
        var bot = SolarSystemShips.Instance.CreateBot(currentPerson, BotBuilder.BotState.Stationary);
        bot.transform.position = pos;

        return bot.gameObject;
    }
}
