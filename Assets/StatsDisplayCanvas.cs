using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsDisplayCanvas : MonoBehaviour
{
    [SerializeField] StatsDisplayCanvasRow speedValue;
    [SerializeField] StatsDisplayCanvasRow fuelValue;
    Player player;
    
    private void Start()
    {
        player = Player.inst;
    }

    private void Update()
    {
        var ship = player.control;
        speedValue.SetValue(ship.speed, player.Ship().data.maxSpeedUnits, $"{ship.moveMode.ToString()} {(ship.speed * World.unitSize).ToString("F0")}");
        fuelValue.SetValue(player.Ship().fuel.value, player.Ship().fuel.max);
    }
}
