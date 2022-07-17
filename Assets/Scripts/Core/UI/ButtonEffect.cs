using Core.Init;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public enum ActionType { None, Over, Selected}
        public ActionType over;
        public void OnPointerEnter(PointerEventData eventData)
        {
            over = ActionType.Over;
            CursorManager.ChangeCursor(CursorManager.CursorType.Action);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            over = ActionType.None;
            CursorManager.ChangeCursor(CursorManager.CursorType.Normal);
        }

        private Image image;
        [SerializeField] Color overColor, noneColor, selectedColor;
        [SerializeField] private Image backImage;
        [SerializeField] private bool withoutBack;
        private float alpha;
        private void Start()
        {
            image = GetComponent<Image>();
            if (!withoutBack)
            {
                backImage.color = noneColor;
                backImage.color *= new Color(1, 1, 1, 0);
            }
        }
        const float TransitionSpeed = 10;
        private void Update()
        {
            if (!withoutBack)
            {
                if (over == ActionType.None)
                {
                    alpha = Mathf.Lerp(alpha, 0, Time.unscaledDeltaTime * TransitionSpeed);
                }
                else
                {
                    alpha = Mathf.Lerp(alpha, 1, Time.unscaledDeltaTime * TransitionSpeed);
                }

                if (backImage != null)
                {
                    backImage.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
                }
            }

            image.color = Color.Lerp(image.color, over == ActionType.Over ? overColor : (over == ActionType.Selected ? selectedColor : noneColor), TransitionSpeed * Time.unscaledDeltaTime);
        }

        public void WithoutLerp()
        {
            if (image == null)
            {
                image = GetComponent<Image>();
            }
            image.color = over == ActionType.Over ? overColor : (over == ActionType.Selected ? selectedColor : noneColor);
        }

        public void SetNoneColor(Color color)
        {
            noneColor = color;
        }


        public void Deselect()
        {
            over = ActionType.None;
        }
    }
}
