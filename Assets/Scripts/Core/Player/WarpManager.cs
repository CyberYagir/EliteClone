using System;
using Core.Galaxy;
using Core.Garage;
using Core.Location;
using Core.Systems;
using UnityEngine;
using UnityEngine.Events;

namespace Core.PlayerScripts
{
    public class WarpManager : MonoBehaviour
    {
        [SerializeField] LocationPoint activeLocationPoint;
        [SerializeField] ParticleSystem warpParticle;
        [SerializeField] private GameObject sceneFader;
        
        
        public bool isWarp;
        public float warpSpeed, maxWarpSpeed, warpSpeedUp, warpSpeedAdd;
        public float maxLocationSpeed = 60;
        public Event<bool> OnChangeWarp;
        
        
        public static float speedToWarp = 1f;


        private WorldDataHandler worldHandler;
        private Player player;

        public void Init(Player player)
        {
            this.player = player;
            worldHandler = PlayerDataManager.Instance.WorldHandler;
        }
        
        private void Update()
        {
            if (activeLocationPoint)
                WarningManager.AddWarning($"Press {InputService.GetData().GetButtonByKAction(KAction.JumpIn)} to enter the station.", WarningTypes.GoLocation);
        
            WarpCheck();
            JumpToLocation();
            JumpFromLocation();
            JumpToSystem();
        }

        public void WarpCheck()
        {
            if (warpSpeed > maxWarpSpeed)
            {
                warpSpeed = maxWarpSpeed;
            }

            if (player.Control.speed < player.Ship().data.maxSpeedUnits / 2f)
            {
                WarpStop();
            }

            if (InputService.GetAxisDown(KAction.StartWarp))
            {
                if (!isWarp)
                {
                    if (player.Control.speed > speedToWarp)
                    {
                        warpParticle.Play();
                        isWarp = true;
                    }
                }
                else
                {
                    if (warpSpeed <= maxWarpSpeed / 3f)
                    {
                        WarpStop();
                    }
                }
            }
        }

        public void JumpToLocation()
        {
            if (InputService.GetAxisDown(KAction.JumpIn))
            {
                if (activeLocationPoint)
                {
                    if (World.Scene == Scenes.System)
                    {
                        warpParticle.Play();
                        SolarStaticBuilder.SaveSystem();
                        if (activeLocationPoint.Location != LocationPoint.LocationType.Scene)
                        {
                            LocationGenerator.SaveLocationFile(activeLocationPoint.Root.name, activeLocationPoint.Location, activeLocationPoint.data);
                            player.SaveData.SetKey("loc_start", true);
                            DontDestroyOnLoad(player);
                            Player.OnPreSceneChanged.Run();
                            World.LoadLevel(Scenes.Location);
                        }
                        else
                        {
                            player.HardStop();
                            player.Control.enabled = false;
                            var fader = Instantiate(sceneFader).GetComponent<FaderMultiScenes>();
                            fader.LoadScene(activeLocationPoint.Scene);
                        }
                    }
                }
            }
        }

        public void JumpFromLocation()
        {
            if (World.Scene == Scenes.Location)
            {
                if (isWarp)
                {
                    if (warpSpeed >= 50)
                    {
                        if (InputService.GetAxisDown(KAction.JumpIn))
                        {
                            JumpFromAnimation();
                        }
                        else
                        {
                            WarningManager.AddWarning("Press \"Jump\" to jump out location", WarningTypes.GoLocation);
                        }
                    }
                }

                if (player.transform.position.magnitude > 2500)
                {
                    JumpFromAnimation();
                }
            }
        }

        void JumpFromAnimation()
        {
            warpParticle.Play();
            DontDestroyOnLoad(player);
            worldHandler.ClarSolarSystem();
            LocationGenerator.RemoveLocationFile();
            Player.OnPreSceneChanged.Run();
            World.LoadLevel(Scenes.System);
        }
    
        public void JumpToSystem()
        {
            if (player.GetTarget() != null)
            {
                if (player.GetTarget().transform.CompareTag("System"))
                {
                    if (isWarp)
                    {
                        if (Vector3.Angle(transform.forward,
                            player.GetTarget().transform.position - transform.position) < 10)
                        {
                            if (warpSpeed >= maxWarpSpeed / 2f)
                            {
                                isWarp = false;
                                OnChangeWarp.Invoke(isWarp);
                                warpSpeed = 0;
                                warpParticle.Play();
                                player.HardStop();
                                DontDestroyOnLoad(player);
                                SolarStaticBuilder.DeleteSystemFile();
                                
                                worldHandler.ChangeSolarSystem(
                                    GalaxyGenerator.systems[player.GetTarget().GetComponent<SolarSystemPoint>().systemName]
                                );
                            
                                Player.OnPreSceneChanged.Run();
                                World.LoadLevel(Scenes.System);
                                return;
                            }

                            warpSpeed += warpSpeedUp * 10 * Time.deltaTime;
                        }
                    }
                }
            }
        }
    
        public void WarpStop()
        {
            if (isWarp)
                warpParticle.Play();
            warpSpeed = 0;
            isWarp = false;
            OnChangeWarp.Invoke(isWarp);
        }

        public void SetActiveLocation(LocationPoint locationPoint)
        {
            if (activeLocationPoint != null)
            {
                if (Vector3.Distance(transform.position, activeLocationPoint.transform.position) < Vector3.Distance(transform.position, locationPoint.transform.position)){
                    activeLocationPoint = locationPoint;
                }
            }
            else
            {
                activeLocationPoint = locationPoint;
            }
        }
        public void RemoveActiveLocation(LocationPoint locationPoint)
        {
            if (activeLocationPoint == locationPoint)
            {
                activeLocationPoint = null;
            }
        }

    }
}
