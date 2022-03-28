using Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Garage
{
    public class GarageDrawEnergyItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text nameT, energyT;
        [SerializeField] private Image imageT;

        public void Init(Item item, KeyPairValue key)
        {
            nameT.text = item.itemName;
            energyT.text = item.GetKeyPair(key).ToString();
            imageT.sprite = item.icon;
        }
    }
}
