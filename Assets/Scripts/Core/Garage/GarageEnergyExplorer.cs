using Core.Game;
using Core.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Garage
{
    public abstract class GarageSlotDataExplorer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image allPowerI, usedPowerI;
        [SerializeField] private GarageDrawEnergyItem item;
        [SerializeField] private RectTransform holder;
        [SerializeField] private RectTransform header, mask;
        [SerializeField] private KeyPairValue key;
        private bool isAddChange;

        private bool over;
        private void Awake()
        {
            GarageDataCollect.OnChangeShip += CalculatePower;
            GarageDataCollect.OnChangeShip += AddEventsOnShip;
        
        }

        public void AddEventsOnShip()
        {
            GarageDataCollect.Instance.cargo.OnChangeInventory += CalculatePower;
            GarageDataCollect.Instance.ship.OnChangeShipData += CalculatePower;
        }

        public class AddInForUsed
        {
            public float used;
            public float all;
        }
        public virtual void AddInFor(Item item, ref AddInForUsed val)
        {
        
        }
    
        public void CalculatePower()
        {
            if (!isAddChange)
            {
                GarageDataCollect.Instance.ship.OnChangeShipData += CalculatePower;
                isAddChange = true;
            }

            var ship = GarageDataCollect.Instance.ship;
            header.transform.parent = null;
            UITweaks.ClearHolder(holder);
            header.transform.parent = holder;

            var power = new AddInForUsed();
        
            for (int i = 0; i < ship.slots.Count; i++)
            {
                if (ship.slots[i].current)
                {
                    var it = Instantiate(item.gameObject, holder).GetComponent<GarageDrawEnergyItem>();
                    it.Init(ship.slots[i].current, key);
                    it.gameObject.SetActive(true);
                
                    AddInFor(ship.slots[i].current, ref power);
                }
            }
            if (power.used > power.all)
            {
                allPowerI.transform.DOScale(new Vector3((power.all / power.used), 1, 1), 1f);
                usedPowerI.transform.DOScale(Vector3.one, 1f);
            }
            else if (power.used < power.all)
            {
                allPowerI.transform.DOScale(Vector3.one, 1f);
                usedPowerI.transform.DOScale(new Vector3((power.used / power.all), 1, 1), 1f);
            }
            else if (power.used == 0 && power.all == 0)
            {
                allPowerI.transform.DOScale(new Vector3(0, 1f, 1f), 1f);
                usedPowerI.transform.DOScale(new Vector3(0, 1f, 1f), 1f);
            }
            else
            {
                allPowerI.transform.DOScale(Vector3.one, 1f);
                usedPowerI.transform.DOScale(Vector3.one, 1f);
            }
        }

        private void Update()
        {
            if (over)
            {
                mask.sizeDelta = Vector2.Lerp(mask.sizeDelta, new Vector2(mask.sizeDelta.x, holder.sizeDelta.y), 10 * Time.deltaTime);
            }
            else
            {
                mask.sizeDelta = Vector2.Lerp(mask.sizeDelta, new Vector2(mask.sizeDelta.x, 0), 10 * Time.deltaTime);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            over = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            over = false;
        }
    }

    public class GarageEnergyExplorer :GarageSlotDataExplorer
    {
        public override void AddInFor(Item item, ref AddInForUsed val)
        {
            base.AddInFor(item, ref val);
            if (item.itemType != ItemType.Generator)
            {
                val.used += Mathf.Abs((float) item.GetKeyPair(KeyPairValue.Energy));
            }
            else
            {
                val.all += Mathf.Abs((float) item.GetKeyPair(KeyPairValue.Energy));
            }
        }
    }
}