using Core.UI;
using UnityEngine;

namespace Core.Garage
{
    public abstract class ShipyardShipsList: MonoBehaviour{

        [SerializeField] protected Shipyard shipyard;

        public void ChangeShipPreview(GameObject shipShipModel, ShipyardItem shipyardItem)
        {
            shipyard.ChangeShipPreview(shipShipModel, shipyardItem);
        }
    }

    public class ShipyardStorage : ShipyardShipsList
    {
        [SerializeField] private Transform holder, item;

        private void Start()
        {
            shipyard.OnChange += UpdateShips;
            UpdateShips();
        }

        private void UpdateShips()
        {
            UITweaks.ClearHolder(holder);
            var ships = GarageDataCollect.Instance.saves.GetStorageShip();

            foreach (var ship in ships)
            {
                if (ship.Key == GarageDataCollect.Instance.playerLocation.locationName)
                {
                    for (int i = 0; i < ship.Value.Count; i++)
                    {
                        var it = Instantiate(item, holder);
                        it.GetComponent<ShipyardItem>().Init(ship.Value[i].GetShip(), false);
                    }
                }
            }
        }
    }
}