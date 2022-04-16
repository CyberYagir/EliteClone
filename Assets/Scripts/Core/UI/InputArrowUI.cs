using Core.PlayerScripts;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class InputArrowUI : MonoBehaviour
    {
        private RectTransform rect;
        private Image arrow, center;
        private void Start()
        {
            rect = GetComponent<RectTransform>();
            arrow = GetComponent<Image>();
            center = transform.parent.GetComponent<Image>();
        }

        private void Update()
        {
            rect.anchoredPosition = new Vector2(Player.inst.control.horizontal, Player.inst.control.vertical) * 100;

            Vector3 diff = rect.anchoredPosition - Vector2.zero;
            diff.Normalize();

            float rotZ = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            rect.localRotation = Quaternion.Euler(0f, 0f, rotZ);

            Color arrowC, centerC;

            if (Vector2.Distance(arrow.rectTransform.anchoredPosition, Vector2.zero) > 40)
            {
                arrowC = new Color(arrow.color.r, arrow.color.g, arrow.color.b, Vector2.Distance(arrow.rectTransform.anchoredPosition, Vector2.zero) / 200f);
                centerC = new Color(center.color.r, center.color.g, center.color.b, 0.5f - arrow.color.a);
            }
            else
            {
                arrowC = new Color(arrow.color.r, arrow.color.g, arrow.color.b, 0);
                centerC = new Color(center.color.r, center.color.g, center.color.b, 1f);
            }

            arrow.color = Color.Lerp(arrow.color, arrowC, 5 * Time.deltaTime);
            center.color = Color.Lerp(center.color, centerC, 5 * Time.deltaTime);
        }
    }
}
