using TMPro;
using UnityEngine;

namespace Core.Garage
{
    public class ShipyardPreview : MonoBehaviour
    {
        [SerializeField] private GameObject buy, sell;
        [SerializeField] private TMP_Text costT;
        private Shipyard shipyard;
    
        public void Start()
        {
            shipyard = GetComponentInParent<Shipyard>();
            shipyard.OnReselect += ChangePreviewData;
        }

        private void ChangePreviewData(ShipyardItem selectedItem)
        {
            if (selectedItem != null)
            {
                buy.SetActive(selectedItem.isMarket);
                sell.SetActive(!selectedItem.isMarket);
            
            
                var cost = 0f;
                if (selectedItem.isMarket)
                    cost = shipyard.GetCost(shipyard.percent);
                else
                    cost = shipyard.GetCost();
                costT.text = "Cost: " + cost.ToString("F2") + " credits";
            }
            else
            {
                buy.SetActive(false);
                sell.SetActive(false);
                costT.text = "";
            }
        }

    }
}
