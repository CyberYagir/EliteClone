using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
        [SerializeField] private RectTransform arrow;
        [SerializeField] private float spacing = 1.5f;
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
            dialoger.OnChangeChoice.AddListener(ChangeChoice);
            dialoger.OnEnd.AddListener(Disable);
        }

        public void Disable()
        {
            dialogUI.SetActive(false);
            dialogReplica.gameObject.SetActive(false);
            choicesHolder.SetActive(false);
            arrow.gameObject.SetActive(false);
        }
        
        public void ThrowReplica(string replica)
        {
            dialogUI.SetActive(true);
            dialogReplica.gameObject.SetActive(true);
            choicesHolder.SetActive(false);
            arrow.gameObject.SetActive(false);
            dialogReplica.text = replica;
        }


        private List<RectTransform> choices = new List<RectTransform>();
        public void ShowChoices(List<ExtendedDialog.NodeReplicaData.TextReplica> replicas)
        {
            
            dialogUI.SetActive(true);
            choicesHolder.SetActive(true);
            arrow.gameObject.SetActive(true);
            
            foreach (Transform tr in choicesHolder.transform)
            {
                Destroy(tr.gameObject);
            }
            choices.Clear();
            
            for (int i = replicas.Count - 1; i >= 0; i--)
            {
                var item = Instantiate(choicePrefab.gameObject, choicesHolder.transform).GetComponent<RectTransform>();
                item.anchoredPosition -= new Vector2(0, item.sizeDelta.y * i * spacing);
                item.GetComponentInChildren<TMP_Text>().text = $"{i + 1}. " + replicas[i].replica;
                choices.Add(item);
            }
            
        }
        
        

        public void ChangeChoice(int num)
        {
            num = (choices.Count-1) - num;
            arrow.DOAnchorPos(new Vector3(choices[num].rect.xMin, choices[num].anchoredPosition.y), 0.2f);
        }
    }
}