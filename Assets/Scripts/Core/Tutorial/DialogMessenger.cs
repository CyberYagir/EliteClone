using System;
using System.Collections;
using System.Collections.Generic;
using Core.Core.Tutorial;
using Core.PlayerScripts;
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
        
        private int replica = -1;
        private Action onClose;
        private List<GameObject> replicas = new List<GameObject>();
        private TutorialsManager tutorialsManager;
        
        public void Init(Action OnClose)
        {
            tutorialsManager = PlayerDataManager.Instance.Services.TutorialsManager;
            
            onClose = OnClose;
            for (int i = 0; i < dialog.replicas.Count; i++)
            {
                var messge = Instantiate(message, holder);
                messge.Init(dialog.replicas[i].text, dialog.replicas[i].character);
                if (dialog.replicas[i].character != Dialog.DialogPart.Character.Player)
                {
                    messge.GetComponent<VerticalLayoutGroup>().childAlignment = TextAnchor.MiddleLeft;
                }
                replicas.Add(messge.gameObject);
            }

            buttonText.text = "Receive";

            NextReplica();
        }

        private void Update()
        {
            holder.GetComponent<VerticalLayoutGroup>().spacing = Random.Range(0.99f, 1f);
        }


        public void NextReplica()
        {
            if (buttonText.text == "Close")
            {
                PlayerTutorial.EnablePlayer(true);
                onClose?.Invoke();
                
                tutorialsManager.Save();
                
                Destroy(gameObject);
                return;
            }
            replica++;
            if (replica < replicas.Count)
            {
                replicas[replica].gameObject.SetActive(true);
                replicas[replica].gameObject.transform.localScale = Vector3.zero;
                replicas[replica].gameObject.transform.DOScale(Vector3.one, 0.1f);
                holder.DOAnchorPosY(holder.sizeDelta.y/2f, 0.1f);
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
