using System.Collections.Generic;
using Core.TDS;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Core.Demo
{
    public class DemoQuests : MonoBehaviour
    {

        public class StillWeaponQuest : TDSQuest
        {

            public StillWeaponQuest()
            {
                text = "Steal weapons from guards.";
            }
            public override void Calculate()
            {
                base.Calculate();
                if (ShooterPlayer.Instance.inventory.items.Count != 0)
                {
                    isCompleted = true;
                }
            }
        }

        public class KillAllGuards:TDSQuest
        {
            public List<Shooter> units = new List<Shooter>();
            public List<Animator> slaves = new List<Animator>();
            public KillAllGuards(List<Shooter> units, List<Animator> slaves)
            {
                this.units = units;
                this.slaves = slaves;
                for (int i = 0; i < this.slaves.Count; i++)
                {
                    slaves[i].speed = Random.Range(0.6f, 1.4f);
                }
                text = "Kill all guards.";
            }

            public override void Calculate()
            {
                base.Calculate();

                foreach (var anim in slaves)
                {
                    anim.Play("LaingTo");
                }
                
                bool allKilled = true;
                for (int i = 0; i < units.Count; i++)
                {
                    if (!units[i].isDead)
                    {
                        allKilled = false;
                    }
                }
                isCompleted = allKilled;
            }
        }
        
        
        public class EnterToShipYardQuest: TDSQuest
        {
            public EnterToShipYardQuest(DemoShipYardEnter enter)
            {
                enter.OnEnter.AddListener(delegate
                {
                    isCompleted = true;
                });

                text = "Break into ShipYard";
            }
        }
        public class OpenDoorQuest: TDSQuest
        {
            private DemoDoor demoDoor;
            public OpenDoorQuest(DemoDoor door)
            {
                demoDoor = door;
                demoDoor.OnOpen.AddListener(delegate
                {
                    isCompleted = true;
                });
                
                text = "Exit to the shipyard.";
            }

            public override void Calculate()
            {
                base.Calculate();
                demoDoor.enable = true;
            }
        }
        
        public class SitInShip:TDSQuest
        {
            public SitInShip()
            {
                text = "Sit in a spaceship and go out into space.";
            }
        }


        [SerializeField] private DemoDoor door;
        [SerializeField] private List<Shooter> units = new List<Shooter>();
        [SerializeField] private List<Animator> slaves = new List<Animator>();
        [SerializeField] private DemoShipYardEnter shipYardEnter;
        private List<TDSQuest> quests = new List<TDSQuest>();
        private int currentQuest;

        public Event<TDSQuest> OnChangeQuest;


        private void Start()
        {
            quests.Add(new StillWeaponQuest());
            quests.Add(new KillAllGuards(units, slaves));
            quests.Add(new OpenDoorQuest(door));
            quests.Add(new EnterToShipYardQuest(shipYardEnter));
            quests.Add(new SitInShip());
            OnChangeQuest.Run(quests[currentQuest]);
        }

        private void FixedUpdate()
        {
            if (currentQuest < quests.Count)
            {
                quests[currentQuest].Calculate();
                if (quests[currentQuest].IsComplited)
                {
                    currentQuest++;
                    if (currentQuest < quests.Count)
                    {
                        OnChangeQuest.Run(quests[currentQuest]);
                    }
                    else
                    {
                        OnChangeQuest.Run(null);
                    }
                }
            }
        }
    }
}
