using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GarageSlotInfo : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [System.Serializable]
    private class GarageSlotInfoUIData
    {
        public TMP_Text slotText, slotLevel, slotType;
        public Image preview;
        public ButtonEffect manageButton;
        public ItemReplacer replacer;
    }

    [SerializeField] private GarageSlotInfoUIData options;
    
    
    
    private RectTransform rect;
    private float time;
    private bool close;
    
    
    
    public bool over;
    public RectTransform last;
    public GarageSlotItem lastItem;

    [HideInInspector]
    public Event<GarageSlotInfo> OnChange = new Event<GarageSlotInfo>();
    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (close && !over)
        {
            CloseCooldown();
        }
        else
        {
            OverWindow();
        }
        UpdateInfo();
    }

    public void CloseCooldown()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            rect.localScale = Vector3.Lerp(rect.localScale, Vector3.zero, Time.deltaTime * 10f);
            if (rect.localScale.magnitude < 0.1f)
            {
                if (lastItem != null)
                {
                    lastItem.Deselect();
                    lastItem = null;
                }
                gameObject.SetActive(false);
            }
        }
    }

    public void OverWindow()
    {
        rect.localScale = Vector3.Lerp(rect.localScale, Vector3.one, Time.deltaTime * 10f);
        if (lastItem != null)
        {
            lastItem.Select();
        }
        time = 0;
    }
    

    public void UpdateInfo()
    {
        if (gameObject.activeSelf)
        {
            if (rect != null)
            {
                if (last != null)
                {
                    rect.position = Vector3.Lerp(rect.position, last.position, 5 * Time.deltaTime);
                }

                ManageButton();
                CheckRealTimeDrop();
            }
        }
    }

    public void CheckRealTimeDrop()
    {
        var isActive = DragManager.Instance.dragObject != null && lastItem.slot.slotType == DragManager.Instance.dragObject.GetData().itemType;
        if (lastItem && isActive)
        {
            options.replacer.enabled = isActive;
        }
        else
        {
            options.replacer.Disable();
        }
    }

    public void ManageButton()
    {
        options.manageButton.gameObject.SetActive(lastItem != null && ItemReplacer.selectedSlot != lastItem.slot);
        if (!options.manageButton.gameObject.activeSelf)
        {
            options.manageButton.over = ButtonEffect.ActionType.None;
        }
    }
    
    
    public void CloseObject()
    {
        if (lastItem != null)
        {
            over = false;
            time = 2;
            lastItem.Deselect();
            lastItem = null;
        }
        
        ItemReplacer.SetSelected(null);
    }
    
    public void Init(GarageSlotItem item)
    {
        var slot = item.slot;

        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }

        time = 0;
        close = false;
        options.slotText.text = $"Slot [{slot.uid}]";
        options.slotLevel.text = "Level: " + slot.slotLevel.ToString();
        options.slotType.text = "Type: " + slot.slotType.ToString();
        options.preview.sprite = slot.current.icon;
        last = item.GetPoint();
        lastItem = item;
        var pos = new Vector3(last.anchoredPosition.x + last.sizeDelta.x + 20, last.anchoredPosition.y, 0);

        if (!gameObject.activeSelf)
        {
            rect.localScale = Vector3.zero;
            rect.DOScale(Vector3.one, 0.5f);
            rect.anchoredPosition = pos;
            gameObject.SetActive(true);
        }

        OnChange.Run(this);
    }

    public void Close()
    {
        close = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }
}
