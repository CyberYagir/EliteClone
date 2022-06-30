using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Core.Dialogs.Game.UI
{
    public class DialogUIPlayer : Singleton<DialogUIPlayer>
    {
        [SerializeField] private GameObject dialogUI;
        [SerializeField] private TMP_Text dialogReplica;
        [SerializeField] private GameObject choicesHolder;
        [SerializeField] private GameObject choicePrefab;
        private void Awake()
        {
            Single(this);
            var dialogers = FindObjectsOfType<Dialoger>();
            for (int i = 0; i < dialogers.Length; i++)
            {
                dialogers[i].OnInit.AddListener(Init);
            }
        }

        public void Init(Dialoger dialoger)
        {
            dialoger.OnThrowReplica.AddListener(ThrowReplica);
            dialoger.OnShowChoice.AddListener(ShowChoices);
        }

        public void ThrowReplica(string replica)
        {
            dialogUI.SetActive(true);
            dialogReplica.gameObject.SetActive(true);
            choicesHolder.SetActive(false);
            dialogReplica.text = replica;
        }


        public void ShowChoices(List<ExtendedDialog.NodeReplicaData.TextReplica> replicas)
        {
            
            dialogUI.SetActive(true);
            choicesHolder.SetActive(true);
            
            foreach (Transform tr in choicesHolder.transform)
            {
                Destroy(tr.gameObject);
            }

            for (int i = replicas.Count - 1; i >= 0; i--)
            {
                var item = Instantiate(choicePrefab.gameObject, choicesHolder.transform).GetComponent<RectTransform>();
                item.anchoredPosition -= new Vector2(0, item.sizeDelta.y * i) + Vector2.up * 20;
                item.GetComponentInChildren<TMP_Text>().text = $"{i + 1}. " + replicas[i].replica;
            }
        }
    }
}