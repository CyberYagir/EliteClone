using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class BaseUIOverlay : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private UpDownUI behaviour;
        private Color color;

        private void Start()
        {
            color = image.color;
        }

        private void Update()
        {
            image.color = Color.Lerp(image.color, behaviour.enabled ? new Color(0, 0, 0, 0): color, 10 * Time.deltaTime);
            image.raycastTarget = !behaviour.enabled;
        }
    }
}
