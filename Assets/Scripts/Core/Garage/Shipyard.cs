using UnityEngine;

namespace Core.Garage
{
    public class Shipyard : MonoBehaviour
    {
        [SerializeField] private ShipyardItem selectedItem;
        [SerializeField] private Transform previewHolder;
        [SerializeField] private ShipyardError throwError;
        private TradeManager tradeManager;

        public Event OnChange = new Event();
        public Event<ShipyardItem> OnReselect = new Event<ShipyardItem>();

        public float percent = 0.02f;
    
        private void Start()
        {
            tradeManager = FindObjectOfType<TradeManager>();
        }

        public void SelectItem(ShipyardItem item)
        {
            selectedItem = item;
            OnReselect.Run(item);
            if (item == null || item.isMarket)
                OnChange.Run();
        }

        public void ChangeShipPreview(GameObject shipMesh, ShipyardItem shipItem)
        {
            foreach (Transform childs in previewHolder)
            {
                Destroy(childs.gameObject);
            }

            var ship = Instantiate(shipMesh, previewHolder);
            ship.gameObject.layer = LayerMask.NameToLayer("ShipUI");
            ship.transform.localEulerAngles += Vector3.up * 180;

            var centerHolder = new GameObject("Holder")
            {
                transform =
                {
                    position = ship.GetComponent<Renderer>().bounds.center
                }
            };
            ship.transform.parent = centerHolder.transform;
            centerHolder.transform.parent = previewHolder;
        
            centerHolder.transform.localPosition = Vector3.zero;
        
            SelectItem(shipItem);
        }

        public float GetCost(float addPercent = 0)
        {
            if (selectedItem != null)
            {
                var cost = 0f;
                var rnd = new System.Random(GarageDataCollect.Instance.stationSeed);
                var item = ItemsManager.GetShipCost(selectedItem.GetShip());
                cost += rnd.Next((int) item.cost.Min, (int) item.cost.Max);
                var slots = selectedItem.GetShip().slots;
                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].current)
                    {
                        cost += tradeManager.GetItemCost(slots[i].current.id.idname);
                    }
                }

                return cost + (cost * addPercent);
            }

            return 0;
        }

        public void Buy()
        {
            var item = GetCost(percent);
            if (GarageDataCollect.Instance.cargo.RemoveCredits(item))
            {
                GarageDataCollect.Instance.saves.AddStorageShip(GarageDataCollect.Instance.playerLocation.locationName, selectedItem.GetShip());
            }
            OnChange.Run();
        }

        public void Sell()
        {
            var item = GetCost();
            if (GarageDataCollect.Instance.saves.RemoveStorageShip(GarageDataCollect.Instance.playerLocation.locationName, selectedItem.GetShip()))
            {
                GarageDataCollect.Instance.cargo.AddCredits(item);
            }
            OnChange.Run();
        }

        public void TriggerChange()
        {
            OnChange.Run();
        }

        public void EquipCurrentShip()
        {
            if (!selectedItem.isMarket)
            {
                if (GarageDataCollect.Instance.cargo.tons > selectedItem.GetShip().data.maxCargoWeight)
                {
                    throwError.ThrowError("The ship you wish to change to will not be able to accommodate the cargo you currently have.");
                    return;
                }
                var garage = GarageDataCollect.Instance;
                var oldShip = garage.ship;
                GarageDataCollect.Instance.saves.RemoveStorageShip(GarageDataCollect.Instance.playerLocation.locationName, selectedItem.GetShip());
                GarageDataCollect.Instance.saves.AddStorageShip(GarageDataCollect.Instance.playerLocation.locationName, oldShip);
                garage.ChangeShip(selectedItem.GetShip());
                OnChange.Run();
                SelectItem(null);
            }
        }
    }
}
