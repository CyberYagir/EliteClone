using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemoTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    private float time;
    private void LateUpdate()
    {
        time += Time.deltaTime;
        text.text = time.ToString();
    }
}
