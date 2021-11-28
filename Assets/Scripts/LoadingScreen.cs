using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public TMP_Text text;
    
    private void Update()
    {
        text.text = (PlayerDataManager.generateProgress * 100).ToString("F1") + "%";
    }
}
