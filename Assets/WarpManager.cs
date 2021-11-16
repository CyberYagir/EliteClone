using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    [SerializeField] LocationPoint activeLocationPoint;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Player.inst.GetTarget())
                transform.position = Player.inst.GetTarget().transform.position;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (activeLocationPoint)
            {
                SolarSystemGenerator.SaveSystem(true);
                DontDestroyOnLoad(Player.inst);
                Application.LoadLevel("Location");
            }
        }
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
