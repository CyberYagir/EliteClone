using System;
using System.Collections;
using System.Collections.Generic;
using Core.Dialogs;
using TMPro;
using UnityEngine;

namespace Core.CommunistsBase.OutDemo
{
    public class LinearDialog : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Dialog dialog;

        private int phrase = -1;

        private void Start()
        {
            phrase = -1;
            text.text = "";
        }

        public void Next()
        {
            phrase++;
            text.text = dialog.replicas[phrase].text;
        }
    }
}
