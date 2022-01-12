using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceCanvasItem : MonoBehaviour
{
    [SerializeField] private Image image, selection;
    [SerializeField] private TMP_Text text;

    public void Init(WorldSpaceObject wso)
    {
        image.sprite = wso.icon;
        text.text = wso.transform.name;
    }

    public void SetSelect(bool state)
    {
        selection.gameObject.SetActive(state);
    }
}
