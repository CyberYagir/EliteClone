using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestItemUI : MonoBehaviour
{
    [SerializeField] private Image fractionImage;
    [SerializeField] private TMP_Text fractionName;


    public void Init(Sprite fractionIcon, string fname)
    {
        fractionImage.sprite = fractionIcon;
        fractionImage.enabled = true;
        fractionName.text = fname;
        fractionName.enabled = true;
    }
}
