using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core
{
    public class DemoTODO : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [TextArea] 
        [SerializeField] private string textChars;
        [SerializeField] private float charCooldown;

        private int index;
        private float time;
        private void Update()
        {
            time += Time.deltaTime;
            if (time > charCooldown && index < textChars.Length)
            {
                text.text += textChars[index];
                index++;
                time = 0;
            }
        }
    }
}
