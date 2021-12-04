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

        if (activeLocationPoint)
        {
            WarningManager.AddWarning("Press 'Jump' to enter the station.", WarningTypes.GoLocation);
        }
        if (InputM.GetAxisDown(KAction.JumpIn))
        {
            if (activeLocationPoint)
            {
                if (Application.loadedLevelName == "System")
                {
                    warpParticle.Play();
                    SolarSystemGenerator.SaveSystem(true);
                    
                    LocationGenerator.SaveLocationFile(activeLocationPoint.root.name);
                    
                    Player.inst.saves.SetKey("loc_start", true);
                    DontDestroyOnLoad(Player.inst);
                    Application.LoadLevel("Location");
                }
            }
        }

        if (Application.loadedLevelName == "Location")
        {
            if (isWarp)
            {
                if (warpSpeed >= 50)
                {
                    warpParticle.Play();
                    DontDestroyOnLoad(Player.inst);
                    PlayerDataManager.currentSolarSystem = null;
                    LocationGenerator.RemoveLocationFile();
                    Application.LoadLevel("System");
                }
            }
        }
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
                            PlayerDataManager.currentSolarSystem =
                                GalaxyGenerator.systems[
                                    Player.inst.GetTarget().GetComponent<SolarSystemPoint>().systemName];
                            Application.LoadLevel("System");
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
    public LocationPoint GetActiveLocation()
    {
        return activeLocationPoint;
    }
}
