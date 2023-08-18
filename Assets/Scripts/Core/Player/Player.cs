using System.Collections.Generic;
using Core.Core.Tutorial;
using Core.Galaxy;
using Core.Game;
using Core.Map;
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
        [SerializeField] private GalacticPathfinder pathfinder;
        public ShipController Control { get; private set; }
        public WarpManager WarpManager { get; private set; }
        public SaveLoadData SaveData { get; private set; }
        public LandManager LandManager { get; private set; }
        public AppliedQuests AppliedQuests { get; private set; }
        public Cargo Cargo { get; private set; }
        public ShipAttack Attack { get; private set; }

        public Damager Damager { get; private set; }

        public TargetManager TargetsManager { get; private set; }
        public ReputationManager ReputationManager { get; private set; }

        public GalacticPathfinder GalaxyFinder => pathfinder;

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
            Control.SetZero();
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
            GetComponent<PlayerTutorial>().Init();
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
                Control = GetComponent<ShipController>();
                Cargo = GetComponent<Cargo>();
                TargetsManager = GetComponent<TargetManager>();
                WarpManager = GetComponent<WarpManager>();
                SaveData = GetComponent<SaveLoadData>();
                LandManager = GetComponent<LandManager>();
                AppliedQuests = GetComponent<AppliedQuests>();
                models = GetComponent<ShipModels>();
                Attack = GetComponent<ShipAttack>();
                Damager = GetComponent<Damager>();
                ReputationManager = GetComponent<ReputationManager>();
                
                
                
                WarpManager.Init(this);
                
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



        public List<ContactObject> GetContacts() => TargetsManager.contacts;

        public void TakeDamage(float damage)
        {
            var ship = spaceShip.GetShip();
            if (ship.GetValue(ItemShip.ShipValuesTypes.Shields).value <= 0)
            {
                TakeDamageHeath(damage);
            }
            else
            {
                Damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Shields).value, damage);
            }
        }

        public void TakeDamageHeath(float damage)
        {
            var ship = spaceShip.GetShip();
            Damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Health).value, damage);
            if (ship.GetValue(ItemShip.ShipValuesTypes.Health).value <= 0)
            {
                World.LoadLevel(Scenes.Death);
            }
        }

        public Camera GetCamera() => Control.Camera;
        public GalaxyObject GetTarget() => TargetsManager.target;
        public void SetTarget(GalaxyObject target) => TargetsManager.SetTarget(target);
        public ItemShip Ship() => spaceShip.GetShip();
        public void HardStop() => Control.HardStop();
        
    }
}