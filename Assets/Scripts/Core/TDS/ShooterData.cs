using Core;
using Core.PlayerScripts;
using UnityEngine;
using Event = Core.Event;

public class ShooterData : MonoBehaviour, IDamagable
{
    public float heath { get; private set; } = 100;
    public float energy { get; private set; } = 1000;
    private float maxHeath = 100;
    private float maxEnergy  = 1000;

    public Event UpdateData = new Event();
    

    public void TakeDamage(float damage)
    {
        
    }


    public bool RemoveEnergy(float val)
    {
        if (energy > val)
        {
            energy -= val;
            UpdateData.Run();
            return true;
        }

        return false;
    }

    public float GetHealth()
    {
        return heath / maxHeath;
    }
    
    public float GetEnergy()
    {
        return energy / maxEnergy;
    }
}
