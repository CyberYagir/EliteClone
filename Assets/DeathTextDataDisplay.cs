using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathTextDataDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private float addCharTime;
    private string textTemplate = "Result: Destruction of the vehicle.\nCapsule heading to station: [%stationName%]\n\nData and cargo: lost.\n\nCommitments: remain";
    private int textLetter;
    private float time;
    private bool isInited;
    public void Update()
    {
        time += Time.deltaTime;
        if (DeathDataCollector.Instance.findNearStation == null)
        {
            text.text = "Wait for data...";
        }
        else
        {
            if (!isInited)
            {
                text.text = "";
                textTemplate = textTemplate.Replace("%stationName%", DeathDataCollector.Instance.findNearStation.name);
                isInited = true;
            }
            if (time > addCharTime)
            {
                if (textLetter + 1 <= textTemplate.Length)
                {
                    text.text += textTemplate[textLetter];
                    textLetter++;
                    addCharTime = 0;
                }
            }
        }

    }
}
