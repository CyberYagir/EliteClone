using Core.Game;
using Core.Init;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Garage
{
    public abstract class Draggable : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
    {
        [SerializeField] protected bool over, clicked;
        protected Sprite dragImage;
        protected Item data;

        private Vector2 startPos;


        public Item GetData() => data;

        private void LateUpdate()
        {
            if (clicked)
            {
                if (Vector2.Distance(startPos, Input.mousePosition) > GetComponent<RectTransform>().sizeDelta.y)
                {
                    DragManager.Instance.SetDrag(this, dragImage);
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    clicked = false;
                    CursorManager.ChangeCursor(CursorManager.CursorType.Normal);
                }
            }
        }

        public virtual void Init(Sprite sprite, Item data)
        {
            dragImage = sprite;
            this.data = data;
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            over = true;
            CursorManager.ChangeCursor(CursorManager.CursorType.Action);
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            over = false;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (over)
            {
                startPos = Input.mousePosition;
                clicked = true;
            }
        }
    
    
        public virtual void StartDrag(){}

        public virtual void StopDrag()
        {
            over = false;
            clicked = false;
        }
    
        public virtual void Clicked(){}


        public void OnPointerUp(PointerEventData eventData)
        {
            if (over && clicked && Vector2.Distance(startPos, Input.mousePosition) < 2)
                Clicked();
        }
    }
}