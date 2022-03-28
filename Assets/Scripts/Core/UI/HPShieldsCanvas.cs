using Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class HPShieldsCanvas : MonoBehaviour
    {
        [System.Serializable]
        public class Bar
        {
            public Image img;
            public TMP_Text text;
        }
        [SerializeField] Bar hp, shield;
        private void Update()
        {
            var hpValue = Player.Player.inst.Ship().GetValue(ItemShip.ShipValuesTypes.Health);
            hp.img.fillAmount = (hpValue.value / hpValue.max);
            hp.text.text = "Corpus " + (int)(hp.img.fillAmount * 100) + "%";
        
            var shValue = Player.Player.inst.Ship().GetValue(ItemShip.ShipValuesTypes.Shields);
            shield.img.fillAmount = (shValue.value / shValue.max);
            shield.text.text = "Shields " + (int)(shield.img.fillAmount * 100) + "%";
        }
    }
}
