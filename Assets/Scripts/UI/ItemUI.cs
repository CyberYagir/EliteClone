using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text text; 
    public Item item;

    public void Init(Item it)
    {
        item = it;

        image.sprite = it.icon;
        if (it.IsHaveKeyPair(KeyPairValue.Mineral))
        {
            text.text = it.itemName + $" [{(int)((it.amount.Value/it.amount.Max) * 100)}%]";
        }
        else
        {
            text.text = it.itemName + $" [{it.amount.Value}]";
        }
    }
}
