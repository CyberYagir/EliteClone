using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GarageDragDropItem : Draggable
{
    [SerializeField] private Image image;


    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
    
    public override void StartDrag()
    {
        base.StartDrag();
        gameObject.SetActive(false);
    }

    public override void StopDrag()
    {
        base.StopDrag();
        gameObject.SetActive(true);
    }
}
