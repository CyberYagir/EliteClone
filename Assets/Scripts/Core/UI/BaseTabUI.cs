using UnityEngine;

namespace Core.UI
{
    public class BaseTabUI: MonoUI
    {
        [SerializeField] public GameObject enableOverlay;
        [SerializeField] public UpDownUI upDownUI;
    
        public void Enable()
        {
            enabled = true;
            upDownUI.enabled = true;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (enabled)
            {
                upDownUI.UpdateObject();
            }
        }

        public void Disable()
        {
            enabled = false;
            upDownUI.enabled = false;
        }
    }
}