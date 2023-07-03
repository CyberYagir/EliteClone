using System;
using Core.Game;
using Core.PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class HPShieldsCanvas : MonoUI
    {
        [Serializable]
        public class Bar
        {
            public Image img;
            public TMP_Text text;
        }
        [SerializeField] Bar hp, shield;

        public override void OnUpdate()
        {
            var hpValue = WorldDataHandler.ShipPlayer.Ship().GetValue(ItemShip.ShipValuesTypes.Health);
            hp.img.fillAmount = (hpValue.value / hpValue.max);
            hp.text.text = "Corpus " + (int) (hp.img.fillAmount * 100) + "%";

            var shValue = WorldDataHandler.ShipPlayer.Ship().GetValue(ItemShip.ShipValuesTypes.Shields);
            shield.img.fillAmount = (shValue.value / shValue.max);
            shield.text.text = "Shields " + (int) (shield.img.fillAmount * 100) + "%";
        }
    }
}
