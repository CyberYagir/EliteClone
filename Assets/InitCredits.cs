using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InitCredits : MonoBehaviour
{
    [SerializeField] private TMP_Text version;

    private void Start()
    {
        version.text = "v" + Application.version;
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
