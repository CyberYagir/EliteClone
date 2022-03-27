using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    
    private void Update()
    {
        text.text = (PlayerDataManager.GenerateProgress * 100).ToString("F1") + "%";
    }
}
