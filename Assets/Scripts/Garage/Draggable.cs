using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

public abstract class Draggable : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler
{
    [SerializeField] protected bool over, clicked;
    protected Sprite dragImage;
    protected Object data;

    private Vector2 startPos;
    
    private void LateUpdate()
    {
        if (clicked)
        {
            if (Vector2.Distance(startPos, Input.mousePosition) > GetComponent<RectTransform>().sizeDelta.x)
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

    public virtual void Init(Sprite sprite, Object data)
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


}