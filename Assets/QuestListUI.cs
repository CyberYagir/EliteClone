using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListUI : MonoBehaviour
{
    [SerializeField] private UpDownUI updown;



    public void UpdateQuests()
    {
        
    }
    
    public void Enable()
    {
        this.enabled = true;
        updown.enabled = true;
    }
    public void Disable()
    {
        this.enabled = false;
        updown.enabled = false;
    }
}
