using Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Garage
{
    public class ShipyardItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text shipName;
        [SerializeField] private TMP_Text tons;

        [SerializeField] private ShipyardItemStats stats;
        [SerializeField] private ShipyardItemMoving moving;

        [SerializeField] private Transform slotsHolder, slotItem;


        public bool isMarket;
        [System.Serializable]
        class ShipyardItemStats
        {
            [SerializeField] private TMP_Text fuel, health, temperature, shieds;

            public void Init(ItemShip newShip)
            {

                SetText(ItemShip.ShipValuesTypes.Fuel, fuel, newShip);
                SetText(ItemShip.ShipValuesTypes.Health, health, newShip);
                SetText(ItemShip.ShipValuesTypes.Shields, shieds, newShip);
                SetText(ItemShip.ShipValuesTypes.Temperature, temperature, newShip);
            }

            public void SetText(ItemShip.ShipValuesTypes type, TMP_Text text, ItemShip ship)
            {
                if (ship.shipValues.ContainsKey(type))
                {
                    text.text = ship.shipValues[type].max.ToString();
                }
                else
                {
                    text.gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
        }

        [System.Serializable]
        class ShipyardItemMoving
        {
            [SerializeField] private TMP_Text yaw, pitch, roll, speed, speedUp;


            public void Init(ItemShip newShip)
            {
                roll.text = newShip.data.ZRotSpeed.ToString();
                pitch.text = newShip.data.XRotSpeed.ToString();
                yaw.text = newShip.data.YRotSpeed.ToString();

                speed.text = (newShip.data.maxSpeedUnits * World.unitSize).ToString();
                speedUp.text = (newShip.data.speedUpMultiplier * World.unitSize).ToString();
            
            }
        }

        [SerializeField] private ItemShip ship;

        public ItemShip GetShip() => ship;
    
        public void Init(ItemShip newShip, bool isMark)
        {
            ship = newShip;

            shipName.text = ship.shipName;
            tons.text = ship.data.maxCargoWeight.ToString();
        
            stats.Init(ship);
            moving.Init(ship);


            for (int i = 0; i < ship.slots.Count; i++)
            {
                var slot = Instantiate(slotItem.gameObject, slotsHolder);
                slot.transform.GetChild(0).GetComponent<Image>().sprite = ship.slots[i].current.icon;
                slot.transform.GetChild(1).GetComponent<TMP_Text>().text = "L" + ship.slots[i].slotLevel.ToString();
                slot.gameObject.SetActive(true);
            }

            isMarket = isMark;

        
            gameObject.SetActive(true);
            GetComponent<RectTransform>().sizeDelta += Vector2.one; //Костыль чтобы заребилдить так как LayoutRebuilder not working
        
        }



        public void OnClick()
        {
            GetComponentInParent<ShipyardShipsList>().ChangeShipPreview(ship.shipModel, this);
        }
    }
}
