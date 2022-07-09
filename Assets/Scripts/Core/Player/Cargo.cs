using System.Collections.Generic;
using Core.Game;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class Cargo : MonoBehaviour
    {
        public class ItemData
        {
            public string idName;
            public float value;
        }
    
        [SerializeField] private GameObject dropPrefab;
    
        private ItemShip currentShip;
        public List<Item> items { get; private set; } = new List<Item>();
        public Dictionary<int, List<Item>> idDictionary = new Dictionary<int, List<Item>>();
        public float tons { get; private set; }
        public Event OnChangeInventory = new Event();


        private int creditID;
    
        private void Awake()
        {
            var ship = GetComponent<Ship>();
            if (ship != null)
            {
                currentShip = ship.GetShip();
            }

            creditID = ItemsManager.GetItem("credit").id.id;
        }

        public void CustomInit(PlayerData data, ItemShip ship)
        {
            SetShip(ship);
            LoadData(data.items);
        }

        public void SetShip(ItemShip ship)
        {
            currentShip = ship;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                var item = ItemsManager.GetCredits();
                item.amount.SetValue(1000);
                AddItem(item);
            }
        }

        public int GetCredits()
        {
            var credit = FindItem(creditID);
            if (credit != null)
            {
                return (int)credit.amount.Value;
            }

            return 0;
        }

        public void AddCredits(float count)
        {
            var credits = ItemsManager.GetCredits().Clone();
            credits.amount.SetValue(count);
            AddItem(credits);
            OnChangeInventory.Run();
        }
        public bool RemoveCredits(float remove, bool updateInventory = false)
        {
            var credit = FindItem(creditID);
            if (credit != null)
            {
                if (credit.amount.Value >= remove)
                {
                    credit.amount.SubValue(remove);
                    if (updateInventory)
                    {
                        OnChangeInventory.Run();
                    }
                    return true;
                }
            }
            return false;
        }

        public void UpdateInventory()
        {
            OnChangeInventory.Run();
        }
    
    
        public List<ItemData> GetData()
        {
            List<ItemData> itemDatas = new List<ItemData>();
            for (int i = 0; i < items.Count; i++)
            {
                itemDatas.Add(new ItemData {idName = items[i].id.idname, value = items[i].amount.Value});
            }
            return itemDatas;
        }

        public void LoadData(List<ItemData> itemDatas)
        {
            foreach (var data in itemDatas)
            {
                var item = ItemsManager.GetItem(data);
                AddItem(item, false);
            }

            OnChangeInventory.Run();
        }

        public Item FindItem(int id)
        {
            if (idDictionary.ContainsKey(id))
            {
                if (idDictionary[id].Count != 0)
                {
                    return idDictionary[id][0];
                }
            }

            return null;
        }

        public List<Item> FindItems(int id)
        {
            if (idDictionary.ContainsKey(id))
            {
                return idDictionary[id];
            }

            return new List<Item>();
        }

        public bool ContainItem(int id)
        {
            return idDictionary.ContainsKey(id) && idDictionary[id].Count != 0;
        }

        public bool ContainItem(int id, float value)
        {
            var item = FindItem(id);
            if (item)
            {
                if (item.amount.Value >= value)
                {
                    return true;
                }   
            }

            return false;
        }
        public bool ContainItems(List<Item> its)
        {
            var list = new List<Item>(its);
            var cheked = new List<Item>();
            foreach (var item in list)
            {
                if (ContainItem(item.id.id, item.amount.Value))
                {
                    var finded = FindItems(item.id.id);
                    if (finded.Count == 0) return false;
                    for (int i = 0; i < finded.Count; i++)
                    {
                        if (!cheked.Contains(finded[i]))
                        {
                            cheked.Add(finded[i]);
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            if (cheked.Count >= list.Count)
            {
                return true;
            }

            return false;
        }
    
        public bool AddItems(List<Item> its, bool add = true)
        {
            float itemsWeight = 0;
            foreach (var item in its)
            {
                itemsWeight += item.amount.Value * (float)item.GetKeyPair(KeyPairValue.Mass);
            }

            if (tons + itemsWeight < currentShip.data.maxCargoWeight)
            {
                if (add)
                {
                    foreach (var item in its)
                    {
                        AddItem(item, false);
                    }
                }
                OnChangeInventory.Run();
                return true;
            }
            OnChangeInventory.Run();
            return false;
        }
        public bool AddItem(Item item, bool callEvent = true)
        {
            var oldItems = items;
        
            var mass = (float) item.GetKeyPair(KeyPairValue.Mass);
            
            var canAddByWeight = tons + mass <= currentShip.data.maxCargoWeight;
            if (!canAddByWeight)
            {
                return false;
            }

            if (idDictionary.ContainsKey(item.id.id) && idDictionary[item.id.id].Count != 0)
            {
                if (item.amount.Min != 0)
                {
                    item.amount.SetMinZero();
                }

                var findedItem = idDictionary[item.id.id];
                
                for (int i = 0; i < findedItem.Count; i++) 
                {
                    if (findedItem[i].amount.value >= findedItem[i].amount.Max) continue;
                
                    if (canAddByWeight && findedItem[i].amount.value + item.amount.value < findedItem[i].amount.Max)
                    {
                        findedItem[i].amount.value += item.amount.value;
                        tons += mass * item.amount.value;
                        if (callEvent) OnChangeInventory.Run();
                        return true;
                    }

                    var startItemCount = item.amount.value;
                    for (int j = 0; j < startItemCount; j++)
                    {
                        if (findedItem[i].amount.Value + 1 <= findedItem[i].amount.Max && canAddByWeight)
                        {
                            findedItem[i].amount.AddValue(1);
                            item.amount.SubValue(1);
                            tons += mass;
                        }
                        canAddByWeight = tons + mass <= currentShip.data.maxCargoWeight;
                        if (!canAddByWeight)
                        {
                            if (callEvent) OnChangeInventory.Run();
                            break;
                        }
                    }
                    if (item.amount.value == 0)
                    {
                        if (callEvent) OnChangeInventory.Run();
                        return true;
                    }
                }
            }
            var itemMass = item.amount.Value * mass;
            if (tons + itemMass <= currentShip.data.maxCargoWeight)
            {
                AddToInventory(item);
                if (callEvent) OnChangeInventory.Run();
                return true;
            }

            items = oldItems;
            return false;
        }
        public void AddToInventory(Item item)
        {
            items.Add(item.Clone());
            if (!idDictionary.ContainsKey(item.id.id))
            {
                idDictionary.Add(item.id.id, new List<Item>());
            }

            idDictionary[item.id.id].Add(items[items.Count - 1]);
            
            tons +=  item.amount.Value * (float) item.GetKeyPair(KeyPairValue.Mass);
        }

        public Item RemoveItem(int id, float value = 1, bool callEvent = false)
        {
            var item = FindItem(id);
            idDictionary[item.id.id].Remove(item);
            return RemoveItem(item, value, callEvent);
        }

        public Item RemoveItem(Item item, float value = 1, bool callEvent = false)
        {
            if (item == null) return null;
            
            var removed = item.Clone();
            removed.amount.SetValue(0);
            if (item)
            {
                var itemMass = (float) item.GetKeyPair(KeyPairValue.Mass);
                for (int i = 0; i < value; i++)
                {
                    if (item.amount.Value > 0)
                    {
                        item.amount.SubValue(1);
                        tons -= itemMass;
                        removed.amount.AddValue(1);
                    }
                }
                if (item.amount.Value == 0)
                {
                    items.Remove(item);
                    idDictionary[item.id.id].Remove(item);
                    tons -= item.amount.Value * itemMass;
                    Destroy(item);
                    OnChangeInventory.Run();
                }
            }

            if (removed.amount.Value == 0)
            {
                if (callEvent) OnChangeInventory.Run();
                return null;
            }

            if (callEvent) OnChangeInventory.Run();
            return removed;
        }

        public bool RemoveItems(List<Item> its)
        {
            if (ContainItems(its))
            {
                for (int i = 0; i < its.Count; i++)
                {
                    RemoveItem(its[i].id.id, its[i].amount.Value);
                }
                OnChangeInventory.Run();
                return true;
            }
            return false;
        }

        public void DropItem(int id, float currentValue, float val)
        {
            if (ContainItem(id))
            {
                var item = idDictionary[id].Find(x => x.amount.value == currentValue);
                var removed = RemoveItem(item, val);
                if (removed)
                {
                    var drop = Instantiate(dropPrefab, transform.position, transform.rotation).GetComponent<WorldDrop>();
                    Physics.IgnoreCollision(drop.GetComponent<BoxCollider>(), GetComponentInChildren<Collider>(), true);
                    drop.Init(removed, 2);
                    drop.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
                
                    OnChangeInventory.Run();
                }
            }
        }
    }
}
