using System;
using Core.Game;
using Core.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    [Serializable]
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
                PlayerDataManager.Instance.WorldHandler.ShipPlayer.WarpManager.isWarp ? active : desactive,
                10 * Time.deltaTime);
            warpText.color = warpActiveColor;
            warpSpeedText.color = warpActiveColor;
        
            background.color = warpActiveColor;
        }
    }

    public class StatsDisplayCanvas : MonoUI
    {
        [SerializeField] private StatsDisplayCanvasRow speedValue;
        [SerializeField] private StatsDisplayCanvasRow fuelValue;
        [SerializeField] private StatsDisplayCanvasRow warpValue;
        [SerializeField] private StatsDisplayCanvasRow heatValue;
        [SerializeField] private WarpDisplayCanvasRow warpDisplayRow;
    
        private Player player;
        private Color warpActiveColor;
        
        public override void Init()
        {
            base.Init();
            player = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
        }


        public override void OnUpdate()
        {
            base.OnUpdate();
            var ship = player.Control;
            speedValue.SetValue(ship.speed, player.Ship().data.maxSpeedUnits, $"{ship.moveMode.ToString()} {(ship.speed * World.unitSize).ToString("F0")} u/s");
            fuelValue.SetValue(player.Ship().GetValue(ItemShip.ShipValuesTypes.Fuel).value, player.Ship().GetValue(ItemShip.ShipValuesTypes.Fuel).max);
            heatValue.SetValue(player.Ship().GetValue(ItemShip.ShipValuesTypes.Temperature).value, player.Ship().GetValue(ItemShip.ShipValuesTypes.Temperature).max);
            warpValue.SetValue(player.WarpManager.warpSpeed, player.WarpManager.maxWarpSpeed, "Warp speed: " + player.WarpManager.warpSpeed.ToString("F0") + " u/s");
            warpDisplayRow.UpdateColor();
        }
    }
}