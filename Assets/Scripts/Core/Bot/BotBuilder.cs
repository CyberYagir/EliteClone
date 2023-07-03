using System.Text;
using Core.Game;
using Core.Location;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;
using Random = System.Random;

namespace Core.Bot
{
    public sealed class BotBuilder : MonoBehaviour, IDamagable
    {
        public enum BotState
        {
            Attack, Land, Moving, Stationary
        }
        
        public int uniqID = -1;
        [SerializeField] private BotAttackController attackControl;
        [SerializeField] private BotLandController landControl;
        [SerializeField] private BotMovingController movingControl;
        [SerializeField] private BotWeapons weapons;
        [SerializeField] private ShieldActivator shield;
        [SerializeField] private BotVisual visual;
        [SerializeField] private ContactObject contactManager;
        [SerializeField] private ParticleSystem particles;
        [SerializeField] private Ship ship;
        [SerializeField] private GameObject dropPrefab;
        
        private SolarSystemShips.HumanShip human;
        private Damager damager;
        private bool isDead;
        private SolarSystemShips solarSystemShips;
        private WorldDataHandler worldDataHandler;

        public int Fraction => human.fraction;

        private void Awake()
        {
            damager = GetComponent<Damager>();
        }

        public Damager GetDamager() => damager;
        public void SetName() => transform.name = GetVisual().GetShipName() + " [" + transform.name.Split('[')[1];
        public ShieldActivator GetShield() => shield;        
        public BotVisual GetVisual() => visual;
        public void AddContact(bool trigger) => contactManager.Init(trigger);
        public ItemShip GetShip() => ship.GetShip();        
        public void SetLandPoint(LandPoint landPoint)=>landControl.SetLandPoint(landPoint);
        public void SetHuman(SolarSystemShips.HumanShip humanShip) => human = humanShip;



        public void SetShip(ItemShip shipData)
        {
            int weaponsSeed = NamesHolder.StringToSeed(transform.name);
            foreach (var slt in shipData.slots)
            {
                if (slt.slotType == ItemType.Weapon)
                {
                    slt.current = weapons.GetWeapon(weaponsSeed);
                }
            }

            ship.SetShip(shipData);
            SetName();
        }
        

        public void InitBot(WorldDataHandler worldDataHandler, SolarSystemShips solarSystemShips, Random rnd = null)
        {
            this.worldDataHandler = worldDataHandler;            
            this.solarSystemShips = solarSystemShips;

            
            
            NamesHolder.Init();
            var firstName = "";
            var lastName = "";
            if (rnd == null)
            {
                firstName = NamesHolder.ToUpperFist(NamesHolder.Instance.FirstNames[UnityEngine.Random.Range(0, NamesHolder.Instance.FirstNames.Length)]);
                lastName = NamesHolder.ToUpperFist(NamesHolder.Instance.LastNames[UnityEngine.Random.Range(0, NamesHolder.Instance.LastNames.Length)]);
            }
            else
            {
                firstName = NamesHolder.ToUpperFist(NamesHolder.GetFirstName(rnd));
                lastName = NamesHolder.ToUpperFist(NamesHolder.GetLastName(rnd));
            }
            transform.name = GetVisual().GetShipName() + $" [{firstName} {lastName}]";
        }
        public void InitBot(WorldDataHandler worldDataHandler, string first, string last, SolarSystemShips solarSystemShips)
        {
            this.solarSystemShips = solarSystemShips;
            this.worldDataHandler = worldDataHandler;

            
            NamesHolder.Init();
            transform.name = GetComponent<BotVisual>().GetShipName() + $" [{first} {last}]";
        }
        public ParticleSystem PlayWarp()
        {
            particles.Play();
            return particles;
        }
        public void SetBehaviour(BotState botState)
        {
            attackControl.enabled = botState == BotState.Attack;
            landControl.enabled = botState == BotState.Land;
            movingControl.enabled = botState == BotState.Moving;
        }

        
        public void TakeDamage(float damage)
        {
            SetBehaviour(BotState.Attack);
            attackControl.SetTarget(worldDataHandler.ShipPlayer.transform);
            var ship = this.ship.GetShip();
            if (ship.GetValue(ItemShip.ShipValuesTypes.Shields).value <= 0)
            {
                damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Health).value, damage);
                shield.isActive = false;
                if (ship.GetValue(ItemShip.ShipValuesTypes.Health).value <= 0)
                {
                    Death();
                }
            }
            else
            {
                damager.TakeDamage(ref ship.GetValue(ItemShip.ShipValuesTypes.Shields).value, damage);
                shield.isActive = true;
            }
        }


        public void Death()
        {
            if (!isDead)
            {
                if (uniqID != -1)
                {
                    solarSystemShips.AddDead(this);
                }
                else
                {
                    solarSystemShips.ExplodeShip(this);
                }

                Drop();

                isDead = true;
                Destroy(gameObject);
            }
        }

        public void Drop()
        {
            var rnd = new Random(uniqID);
            if (uniqID == -1)
            {
                rnd = new Random();
            }
            var dropCount = rnd.Next(0, 6);
            for (int i = 0; i < dropCount; i++)
            {
                var drop = Instantiate(dropPrefab, transform.position, transform.rotation).GetComponent<WorldDrop>();
                drop.GetComponent<BoxCollider>().isTrigger = true;
                drop.Init(ItemsManager.GetRewardItem(rnd), worldDataHandler);
                if (worldDataHandler.ShipPlayer.quests.quests.Count >= 1)
                {
                    if (human.fraction == WorldDataItem.Fractions.NameToID("Pirates"))
                    {
                        var chance = rnd.Next(0, 100);
                        if (chance <= 40)
                        {
                            drop.Init(ItemsManager.GetItem("transmitter_box"), worldDataHandler);
                        }
                    }
                }
                drop.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.insideUnitSphere, ForceMode.Impulse);
            }   
        }
        
        public void AttackPlayer()
        {
            attackControl.SetTarget(worldDataHandler.ShipPlayer.transform);
            SetBehaviour(BotState.Attack);
        }

    }
}
