using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InitError : MonoBehaviour
{
    [SerializeField] private CustomAnimate animate;
    [SerializeField] private TMP_Text text;
    private void Start()
    {
        if (PlayerPrefs.HasKey("Error"))
        {
            text.text = "Error: " + PlayerPrefs.GetString("Error");
            animate.reverse = false;
            PlayerPrefs.DeleteKey("Error");
        }
    }

    public void Close()
    {
        animate.reverse = true;
    }
}
