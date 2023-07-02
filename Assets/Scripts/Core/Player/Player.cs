using System.Collections.Generic;
using Core.Galaxy;
using Core.Game;
using Core.Systems;
using Core.UI;
using UnityEngine;

namespace Core.PlayerScripts
{
    interface IDamagable
    {
        public void TakeDamage(float damage);
    }

    public class Player : MonoBehaviour, IDamagable
    {
        public ShipController control { get; private set; }
        public WarpManager warp { get; private set; }
        public SaveLoadData saves { get; private set; }
        public LandManager land { get; private set; }
        public AppliedQuests quests { get; private set; }
        public Cargo cargo { get; private set; }
        public ShipAttack attack { get; private set; }

        public Damager damager { get; private set; }

        public TargetManager targets { get; private set; }
        public ReputationManager rep { get; private set; }

        public static Event OnSceneChanged = new Event();
        public static Event OnPreSceneChanged = new Event();

        
        private float coolerValue = -1;
        private ShipModels models;
        private Ship spaceShip;
        private WorldDataHandler worldDataHandler;
        
        private float heatTime;

        private void Awake()
        {
            OnSceneChanged = new Event();
            OnPreSceneChanged = new Event();
            
            worldDataHandler = PlayerDataManager.Instance.WorldHandler;
            
            Init();
        }

        public void AddHeat(float heat)
        {
            var currShip = spaceShip.GetShip();
            var temperature = currShip.GetValue(ItemShip.ShipValuesTypes.Temperature);
            temperature.value += heat * Time.deltaTime;
            if (temperature.value > temperature.max)
            {
                temperature.Clamp();
                TakeDamage(Time.deltaTime);
                WarningManager.AddWarning("Heat level critical!", WarningTypes.Damage);
            }

            if (heat > 0)
                heatTime = 0;
            if (currShip.GetValue(ItemShip.ShipValuesTypes.Temperature).value < 0)
            {
                currShip.GetValue(ItemShip.ShipValuesTypes.Temperature).value = 0;
            }
        }
    
        public void StopAxis()
        {
            control.SetZero();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    
        private void Update()
        {
            DownHeat();
        }

        public void DownHeat()
        {
            heatTime += Time.deltaTime;
            if (heatTime > 2)
            {
                if (coolerValue == -1)
                {
                    coolerValue = (float)Ship().slots.Find(x => x.slotType == ItemType.Cooler).current.GetKeyPair(KeyPairValue.Value);
                }
                AddHeat(-coolerValue);
            }
        }

        private void Start()
        {
            ChangeScene();
        }

        public static void ChangeScene()
        {
            OnSceneChanged.Run(); 
        }

        public void Init()
        {
            
            if (worldDataHandler.ShipPlayer == null)
            {
                worldDataHandler.SetShipPlayer(this);
                
                spaceShip = GetComponent<Ship>();
                GalaxyGenerator.LoadSystems();
                control = GetComponent<ShipController>();
                cargo = GetComponent<Cargo>();
                targets = GetComponent<TargetManager>();
                warp = GetComponent<WarpManager>();
                saves = GetComponent<SaveLoadData>();
                land = GetComponent<LandManager>();
                quests = GetComponent<AppliedQuests>();
                models = GetComponent<ShipModels>();
                attack = GetComponent<ShipAttack>();
                damager = GetComponent<Damager>();
                rep = GetComponent<ReputationManager>();
                
                
                warp.Init(this);
                
                spaceShip.OnChangeShip += models.InitShip;
                spaceShip.SetShip(spaceShip.CloneShip());
            }
        }

    
        public void LoadShip(ShipData data)
        {
            spaceShip.LoadShip(data);
            spaceShip.GetShip().ValuesToDictionary();
            TakeDamage(0);
            TakeDamageHeath(0);
        }



        public List<ContactObject> GetContacts() => targets.contacts;

        public void TakeDamage(float damage)
        {
            var ship = spaceShip.GetShip();
            if (ship.GetValue(ItemShip.ShipValuesTypes.Shields).value <= 0)
            {
                TakeDamageHeath(damage);
            }
            else
            {
                damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Shields).value, damage);
            }
        }

        public void TakeDamageHeath(float damage)
        {
            var ship = spaceShip.GetShip();
            damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Health).value, damage);
            if (ship.GetValue(ItemShip.ShipValuesTypes.Health).value <= 0)
            {
                World.LoadLevel(Scenes.Death);
            }
        }

        public Camera GetCamera() => control.Camera;
        public GalaxyObject GetTarget() => targets.target;
        public void SetTarget(GalaxyObject target) => targets.SetTarget(target);
        public ItemShip Ship() => spaceShip.GetShip();
        public void HardStop() => control.HardStop();

    }
}