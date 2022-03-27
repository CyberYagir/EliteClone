using UnityEngine;

public class BaseTabUI: MonoBehaviour
{
    [SerializeField] public GameObject enableOverlay;
    [SerializeField] public UpDownUI upDownUI;
    
    public void Enable()
    {
        this.enabled = true;
        upDownUI.enabled = true;
    }
    public void Disable()
    {
        enabled = false;
        upDownUI.enabled = false;
    }
}