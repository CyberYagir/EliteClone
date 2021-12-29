using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text;
    private Item item;

    public void Init(Item it)
    {
        item = it;

        image.sprite = it.icon;
        text.text = it.itemName + $" [{it.amount.Value}]";
    }
}
