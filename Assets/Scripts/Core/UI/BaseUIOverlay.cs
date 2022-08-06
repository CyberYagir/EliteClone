using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class BaseUIOverlay : MonoUI
    {
        [SerializeField] private Image image;
        [SerializeField] private UpDownUI behaviour;
        private Color color;

        private void Start()
        {
            color = image.color;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            image.color = Color.Lerp(image.color, behaviour.enabled ? new Color(0, 0, 0, 0): color, 10 * Time.deltaTime);
            image.raycastTarget = !behaviour.enabled;

        }
        
    }
}
