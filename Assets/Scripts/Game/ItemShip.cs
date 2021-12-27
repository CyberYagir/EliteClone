using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class ShipVariables
    {
        public float ZRotSpeed, XRotSpeed, YRotSpeed;
        public float maxSpeedUnits, speedUpMultiplier;
        public int maxCargoWeight;
    }
    [System.Serializable]
    public class ShipClaped
    {
        public float value;
        public float max;
    }

    [System.Serializable]
    public class Slot
    {
        public int uid;
        public ItemType slotType;
        public int slotLevel = 1;
        public Item current;
    }
    [CreateAssetMenu(fileName = "", menuName = "Game/Ship", order = 1)]
    public class ItemShip : ScriptableObject
    {
        public string shipName;
        public Mesh shipModel;
        public GameObject shipCabine;
        public ShipClaped fuel, hp, shields, heat;
        public ShipVariables data;
        public List<Slot> slots;
        public ItemShip Clone()
        {
            return Instantiate(this);
        }
    }
}