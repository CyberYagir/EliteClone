using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsDisplayCanvasRow : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Transform value;


    public void SetValue(float value, float maxValue, string text = "_")
    {
        this.value.localScale = new Vector3(value / maxValue, 1, 1);
        if (text != "_")
        {
            this.text.text = text;
        }
    }
}
