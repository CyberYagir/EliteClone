using System;
using System.Collections;
using System.Collections.Generic;
using Core.Core.Tutorial;
using Core.Tutorial;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Core.Dialogs.Visuals
{
    public class DialogMessenger : MonoBehaviour
    {
        public Dialog dialog { get; set; }

        [SerializeField] private DialogMessengerMessage message;
        [SerializeField] private RectTransform holder;
        [SerializeField] private TMP_Text buttonText;

        private List<GameObject> replicas = new List<GameObject>();

        public void Init()
        {
            for (int i = 0; i < dialog.replicas.Count; i++)
            {
                var messge = Instantiate(message, holder);
                messge.Init(dialog.replicas[i].text, dialog.replicas[i].character);
                if (dialog.replicas[i].character == Dialog.DialogPart.Character.Player)
                {
                    messge.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.MiddleLeft;
                }
                replicas.Add(messge.gameObject);
            }

            buttonText.text = "Receive";
        }

        private void Update()
        {
            holder.GetComponent<VerticalLayoutGroup>().spacing = Random.Range(0.99f, 1f);
        }

        private int replica = -1;
        public void NextReplica()
        {
            if (buttonText.text == "Close")
            {
                PlayerTutorial.Instance.Invoke(dialog.replicas[replica].finishMethodName, 0);
                PlayerTutorial.EnablePlayer(true);
                Destroy(gameObject);
                return;
            }
            replica++;
            if (replica < replicas.Count)
            {
                replicas[replica].gameObject.SetActive(true);
                replicas[replica].gameObject.transform.localScale = Vector3.zero;
                replicas[replica].gameObject.transform.DOScale(Vector3.one, 0.5f);
                holder.DOAnchorPosY(holder.sizeDelta.y/2f, 0.2f);
            }
            if (replica + 1 >= replicas.Count)
            {
                buttonText.text = "Close";
            }
            else if (dialog.replicas[replica].character == Dialog.DialogPart.Character.Player || dialog.replicas[replica].character == Dialog.DialogPart.Character.System)
            {
                buttonText.text = "Receive";
            }
            else
            {
                buttonText.text = "Send";
            }
        }
    }
}
