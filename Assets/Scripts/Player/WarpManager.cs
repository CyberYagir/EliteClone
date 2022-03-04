using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    [SerializeField] LocationPoint activeLocationPoint;
    [SerializeField] ParticleSystem warpParticle;
    public bool isWarp;
    public float warpSpeed, maxWarpSpeed, warpSpeedUp, warpSpeedAdd;

    private void Update()
    {
        if (activeLocationPoint)
            WarningManager.AddWarning("Press 'Jump' to enter the station.", WarningTypes.GoLocation);
        
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

        if (Player.inst.control.speed < Player.inst.Ship().data.maxSpeedUnits / 2f)
        {
            WarpStop();
        }

        if (InputM.GetAxisDown(KAction.StartWarp))
        {
            if (!isWarp)
            {
                if (Player.inst.control.speed > 1f)
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
        if (InputM.GetAxisDown(KAction.JumpIn))
        {
            if (activeLocationPoint)
            {
                if (World.Scene == Scenes.System)
                {
                    warpParticle.Play();
                    SolarSystemGenerator.SaveSystem(true);
                    LocationGenerator.SaveLocationFile(activeLocationPoint.root.name, activeLocationPoint.locationType);
                    Player.inst.saves.SetKey("loc_start", true);
                    DontDestroyOnLoad(Player.inst);
                    Player.OnPreSceneChanged.Run();
                    World.LoadLevel(Scenes.Location);
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
                    if (InputM.GetAxisDown(KAction.JumpIn))
                    {
                        JumpFromAnimation();
                    }
                    else
                    {
                        WarningManager.AddWarning("Press \"Jump\" to jump out location", WarningTypes.GoLocation);
                    }
                }
            }

            if (Player.inst.transform.position.magnitude > 2500)
            {
                JumpFromAnimation();
            }
        }
    }

    void JumpFromAnimation()
    {
        warpParticle.Play();
        DontDestroyOnLoad(Player.inst);
        PlayerDataManager.CurrentSolarSystem = null;
        LocationGenerator.RemoveLocationFile();
        Player.OnPreSceneChanged.Run();
        World.LoadLevel(Scenes.System);
    }
    
    public void JumpToSystem()
    {
        if (Player.inst.GetTarget() != null)
        {
            if (Player.inst.GetTarget().transform.CompareTag("System"))
            {
                if (isWarp)
                {
                    if (Vector3.Angle(transform.forward,
                        Player.inst.GetTarget().transform.position - transform.position) < 10)
                    {
                        if (warpSpeed >= maxWarpSpeed / 2f)
                        {
                            isWarp = false;
                            warpSpeed = 0;
                            warpParticle.Play();
                            Player.inst.HardStop();
                            DontDestroyOnLoad(Player.inst);
                            SolarSystemGenerator.DeleteSystemFile();
                            PlayerDataManager.CurrentSolarSystem = GalaxyGenerator.systems[Player.inst.GetTarget().GetComponent<SolarSystemPoint>().systemName];
                            
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
