using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = System.Object;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance;

    public Draggable dragObject;
    [SerializeField] private RectTransform movableObject;
    [SerializeField] private Image image;
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            StopDrag();
        }

        movableObject.gameObject.SetActive(dragObject != null);
        if (dragObject != null)
        {
            if (CursorManager.currentType != CursorManager.CursorType.Drag)
            {
                CursorManager.ChangeCursor(CursorManager.CursorType.Drag);
            }

            movableObject.position = Input.mousePosition;
        }
    }

    public void StopDrag()
    {
        if (dragObject != null)
        {
            dragObject.StopDrag();
            CursorManager.ChangeCursor(CursorManager.CursorType.Normal);
            dragObject = null;
        }
    }
    

    public bool SetDrag(Draggable obj, Sprite sprite)
    {
        StopDrag();
        dragObject = obj;
        obj.StartDrag();
        image.sprite = sprite;
        CursorManager.ChangeCursor(CursorManager.CursorType.Drag);
        return true;
    }
}
