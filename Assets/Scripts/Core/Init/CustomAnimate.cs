using DG.Tweening;
using UnityEngine;

namespace Core.Init
{
    public class CustomAnimate : MonoBehaviour
    {
        [SerializeField] protected Vector2 endPos;
        [SerializeField] public bool reverse;
        [SerializeField] private float speed;

        protected RectTransform rect;
        protected Vector2 startPos;

        private bool inited;
        protected void Init()
        {
            rect = GetComponent<RectTransform>();
            startPos = rect.anchoredPosition;
            inited = true;
        }

        public void CustomUpdate()
        {
            if (!inited)
            {
                Init();
            }

            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, !reverse ? endPos : startPos, speed * Time.deltaTime);
            if (reverse)
                gameObject.SetActive(Vector2.Distance(rect.anchoredPosition, !reverse ? endPos : startPos) > 5);
            else 
                gameObject.SetActive(true);
        }
    
        public void Show()
        {
            rect.DOLocalMove(endPos, 0.5f);
        }

        public void Hide()
        {
            rect.DOLocalMove(startPos, 0.5f);
        }
    
    }
}
