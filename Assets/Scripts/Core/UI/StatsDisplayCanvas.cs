using System.Collections;
using System.Collections.Generic;
using Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [System.Serializable]
    public class WarpDisplayCanvasRow
    {
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text warpText, warpSpeedText;
    
        private Color warpActiveColor;
        public Color active = new Color(1f, 0.51f, 0, 1f);
        public Color desactive = new Color(1f, 0.51f, 0, 0.14f);
        public void UpdateColor()
        {
            warpActiveColor = Color.Lerp(warpActiveColor,
                Player.Player.inst.warp.isWarp ? active : desactive,
                10 * Time.deltaTime);
            warpText.color = warpActiveColor;
            warpSpeedText.color = warpActiveColor;
        
            background.color = warpActiveColor;
        }
    }

    public class StatsDisplayCanvas : MonoBehaviour
    {
        [SerializeField] private StatsDisplayCanvasRow speedValue;
        [SerializeField] private StatsDisplayCanvasRow fuelValue;
        [SerializeField] private StatsDisplayCanvasRow warpValue;
        [SerializeField] private StatsDisplayCanvasRow heatValue;
        [SerializeField] private WarpDisplayCanvasRow warpDisplayRow;
    
        private Player.Player player;
        private Color warpActiveColor;
        private void Start()
        {
            player = Player.Player.inst;
        }

        private void Update()
        {
            var ship = player.control;
            speedValue.SetValue(ship.speed, player.Ship().data.maxSpeedUnits, $"{ship.moveMode.ToString()} {(ship.speed * World.unitSize).ToString("F0")} u/s");
            fuelValue.SetValue(player.Ship().GetValue(ItemShip.ShipValuesTypes.Fuel).value, player.Ship().GetValue(ItemShip.ShipValuesTypes.Fuel).max);
            heatValue.SetValue(player.Ship().GetValue(ItemShip.ShipValuesTypes.Temperature).value, player.Ship().GetValue(ItemShip.ShipValuesTypes.Temperature).max);
            warpValue.SetValue(player.warp.warpSpeed, player.warp.maxWarpSpeed, "Warp speed: " + player.warp.warpSpeed.ToString("F0") + " u/s");
            warpDisplayRow.UpdateColor();
        }
    }
}