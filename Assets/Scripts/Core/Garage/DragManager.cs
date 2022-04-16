using System.Collections.Generic;
using Core.Game;
using Core.Init;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Garage
{
    public class DragManager : MonoBehaviour
    {
        public static DragManager Instance;

        public Draggable dragObject;
        [SerializeField] private RectTransform movableObject;
        [SerializeField] private Image image;
        [SerializeField] private GraphicRaycaster mainCanvas;
        public Event<DragDropData> OnDrop = new Event<DragDropData>();
        private void Awake()
        {
            Instance = this;
        }
    
        public class DragDropData
        {
            public GameObject hit;
            public Item item;
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

                movableObject.position = Vector3.Lerp(movableObject.position, Input.mousePosition, (Screen.width * 0.01f) * Time.deltaTime);
            }
        }

        public void StopDrag()
        {
            if (dragObject != null)
            {
                var hits = new List<RaycastResult>();
                movableObject.gameObject.SetActive(false);
                mainCanvas.Raycast(new PointerEventData(EventSystem.current){position = Input.mousePosition}, hits);
                if (hits.Count != 0)
                {
                    OnDrop.Run(new DragDropData { hit = hits[0].gameObject, item = dragObject.GetData()});
                }

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
            movableObject.position = Input.mousePosition;
            return true;
        }
    }
}
