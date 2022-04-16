using Core.Game;
using UnityEngine;

namespace Core.PlayerScripts
{
    public class ShipShield : MonoBehaviour
    {
        private Ship ship;

        private float withoutAttackTime;
        private float oldShield;
        private float shieldAdd;
        private void Start()
        {
            ship = GetComponent<Ship>();
            oldShield = ship.GetShip().GetValue(ItemShip.ShipValuesTypes.Shields).value;
            shieldAdd = (float)ship.GetShip().slots.Find(x => x.slotType == ItemType.Shields).current.GetKeyPair(KeyPairValue.Value);
        }

        private void FixedUpdate()
        {
            var shields = ship.GetShip().GetValue(ItemShip.ShipValuesTypes.Shields);
            if (shields.value != oldShield)
            {
                withoutAttackTime = 0;
                oldShield = shields.value;
            }
            else
            {
                withoutAttackTime += Time.fixedDeltaTime;
            }
            if (withoutAttackTime > 30)
            {
                shields.value += shieldAdd * Time.fixedDeltaTime;
                shields.Clamp();
                ship.GetShip().GetValue(ItemShip.ShipValuesTypes.Shields).value = shields.value;
                oldShield = shields.value;
            }
        }
    }
}
