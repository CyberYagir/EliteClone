using System;
using System.Collections;
using System.Collections.Generic;
using Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuesterItemUI : MonoBehaviour
{
    private Fraction fraction;
    
    [Space] 
    [SerializeField] private Image image;
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text questerNameT, fractionNameT;
    private Character character;
    private int itemIndex;

    public void InitQuesterItem(Fraction _fraction, Sprite _frationImage, string _questerName, Character _character, int itemID)
    {
        fraction = _fraction;
        image.sprite = _frationImage;

        questerNameT.text = _questerName;
        fractionNameT.text = fraction.ToString();

        character = _character;

        itemIndex = itemID;
    }

    public Character GetCharacter()
    {
        return character;
    }

    public void SetSelectedButton()
    {
        GetComponentInParent<CharacterList>().upDownUI.ForceChangeSelect(itemIndex);
    }
}
