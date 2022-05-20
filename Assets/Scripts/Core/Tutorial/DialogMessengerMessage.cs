using System.Collections;
using System.Collections.Generic;
using Core.Dialogs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Tutorial
{
    public class DialogMessengerMessage : MonoBehaviour
    {
        [SerializeField] private TMP_Text text, actorName;


        public void Init(string str, Dialog.DialogPart.Character actor)
        {
            text.text = str;
            actorName.text = actor.ToString();
            LayoutRebuilder.ForceRebuildLayoutImmediate(text.rectTransform);
        }
    }
}
