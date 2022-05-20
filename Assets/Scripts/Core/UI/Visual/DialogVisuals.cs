using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Dialogs.Visuals
{
    public class DialogVisuals : MonoBehaviour
    {
        [SerializeField] private TMP_Text thread, protect;

        private void Start()
        {
            thread.text = "Thread: " + Random.Range(0, 9999).ToString("0000");
            protect.text = "Protect: " + Random.Range(0, 9999).ToString("0000");
        }
    }
}
