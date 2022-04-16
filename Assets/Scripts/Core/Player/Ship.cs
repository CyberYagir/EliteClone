using Core.Game;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class Ship : MonoBehaviour
    {
        [SerializeField] private ItemShip ship;
        public Event OnChangeShip = new Event();
    
    
        public void SetShip(ItemShip item)
        {
            ship = item;
            OnChangeShip.Run();
        }
        public ItemShip GetShip()
        {
            return ship;
        }

        public ItemShip CloneShip()
        {
            return ship.Clone(); 
        }

        public void LoadShip(ShipData data)
        {
            SetShip(data.GetShip());
        }
    }
}
