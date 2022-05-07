using System;
using System.Collections;
using System.Collections.Generic;
using Core.Core.Demo;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Core.TDS.UI
{
    public class UIQuestVisual : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Awake()
        {
            text.text = "";
        }

        public void SetQuest(TDSQuest data)
        {
            if (data != null)
            {
                if (text.text == "")
                {
                    text.rectTransform.DOAnchorPosX(0, 0);
                    text.fontStyle = FontStyles.Normal;
                    text.color = new Color(1, 1, 1, 0);
                    text.text = "<sprite=0>" + data.GetText();
                    text.DOFade(1, 0.5f);
                }
                else
                {
                    StartCoroutine(EndAnimation(data));
                }
            }
            else
            {
                StartCoroutine(EndAnimation(null));
            }
        }


        IEnumerator EndAnimation(TDSQuest data)
        {
            text.rectTransform.DOAnchorPosX(-text.rectTransform.sizeDelta.x * 1.5f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            text.text = "";
            if (data != null)
            {
                SetQuest(data);
            }
        }
    }
}
