using Core.Game;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Garage
{
    public class GarageSlotItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [System.Serializable]
        public class GarageSlotItemUIData
        {
            public RectTransform mover;
            public RectTransform select, point;
            public Image image;
            public TMP_Text itemName;
        }

        [SerializeField] private GarageSlotItemUIData options;
        [SerializeField] private float minX, maxX;
        //[SerializeField] private GarageSlotInfo slotInfo;

        public Event OnEnter = new Event(), OnExit = new Event();
    
        [HideInInspector] public Slot slot;
        public bool over;

        public void Init(Item item, Slot slt)
        {
            slot = slt;
            options.itemName.text = item.itemName;
        }
    

        public void CheckDeselect(GarageSlotInfo slotInfo)
        {
            if (slotInfo.last != options.point)
            {
                Deselect();
            }
        }

        public RectTransform GetPoint() => options.point;
    
        private void Start()
        {
            options.select.transform.localScale = new Vector3(0, 1, 1);
        }

        public void SetImage(Sprite img)
        {
            options.image.sprite = img;
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter.Run();
            Select();
        }

        public void Select()
        {
            options.mover.DOLocalMove(new Vector3(maxX, options.mover.localPosition.y, options.mover.localPosition.z), 0.2f);
            options.select.DOScale(new Vector3(1, 1, 1), 0.5f);
            over = true;
        }

        public void Deselect()
        {
            options.mover.DOLocalMove(new Vector3(minX, options.mover.localPosition.y, options.mover.localPosition.z), 0.2f);
            options.select.DOScale(new Vector3(0, 1, 1), 0.5f);
            over = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnExit.Run();
        }
    }
}
