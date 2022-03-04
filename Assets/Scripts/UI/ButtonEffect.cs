using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public enum ActionType { None, Over, Selected}
    public ActionType over;
    public bool notPPUM = false;
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

    private float startPixelsPerUnit;
    private Image image;
    [SerializeField] Color overColor, noneColor, selectedColor;
    private void Start()
    {
        image = GetComponent<Image>();
        startPixelsPerUnit = image.pixelsPerUnitMultiplier;
    }

    private void Update()
    {
        if (!notPPUM)
            image.pixelsPerUnitMultiplier = Mathf.Lerp(image.pixelsPerUnitMultiplier, over == ActionType.Over || over == ActionType.Selected ? 0 : startPixelsPerUnit, 10 * Time.deltaTime);
        image.color = Color.Lerp(image.color, over == ActionType.Over ? overColor : (over == ActionType.Selected ? selectedColor : noneColor), 10 * Time.deltaTime);
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
}
