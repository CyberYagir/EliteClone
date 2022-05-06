using System;
using System.Collections;
using System.Collections.Generic;
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

        public void SetQuest(string data)
        {
            if (text.text == "")
            {
                text.rectTransform.DOAnchorPosX(0, 0);
                text.fontStyle = FontStyles.Normal;
                text.color = new Color(1, 1, 1, 0);
                text.text = "<sprite=0>" + data;
                text.DOFade(1, 0.5f);
            }else
            {
                StartCoroutine(EndAnimation(data));
            }
        }


        IEnumerator EndAnimation(string data)
        {
            text.rectTransform.DOAnchorPosX(-text.rectTransform.sizeDelta.x * 1.5f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            text.text = "";
        }
    }
}
