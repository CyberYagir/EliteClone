using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    bool over;
    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
    }

    float startPPU;
    Image image;
    [SerializeField] Color active, notActive;
    private void Start()
    {
        image = GetComponent<Image>();
        startPPU = image.pixelsPerUnitMultiplier;
    }
    private void Update()
    {
        image.pixelsPerUnitMultiplier = Mathf.Lerp(image.pixelsPerUnitMultiplier, over ? 0 : startPPU, 10 * Time.deltaTime);
        image.color = Color.Lerp(image.color, over ? active : notActive, 10 * Time.deltaTime);
    }
}
