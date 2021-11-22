using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    [SerializeField] LocationPoint activeLocationPoint;
    [SerializeField] ParticleSystem warpParticle;
    public bool isWarp;
    public float warpSpeed, maxWarpSpeed, warpSpeedUp;
    private void Update()
    {
        if (warpSpeed > maxWarpSpeed)
        {
            warpSpeed = maxWarpSpeed;
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
        if (InputM.GetAxisDown(KAction.JumpIn))
        {
            if (activeLocationPoint)
            {
                warpParticle.Play();
                SolarSystemGenerator.SaveSystem(true);
                LocationGenerator.SaveLocationFile(new Location() { systemName = Path.GetFileNameWithoutExtension(SolarSystemGenerator.GetSystemFileName()), locationName = activeLocationPoint.root.name });
                Player.inst.saves.SetKey("loc_start", true);
                DontDestroyOnLoad(Player.inst);
                Application.LoadLevel("Location");
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
