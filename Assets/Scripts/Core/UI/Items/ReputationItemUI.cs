using System.Collections;
using System.Collections.Generic;
using Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class ReputationItemUI : MonoBehaviour
    {
        [SerializeField] private Transform value;
        [SerializeField] private TMP_Text pointsT;
        [SerializeField] private Image icon;

        public void Init(int scores, string fraction, int maxValue)
        {
            var id = WorldDataItem.Fractions.NameToID(fraction);
            if (id != -1)
            {
                icon.sprite = WorldDataItem.Fractions.IconByID(id);
                pointsT.text = scores + "R";
                value.transform.localScale = new Vector3(1, scores / (float) maxValue, 1);
                gameObject.SetActive(true);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
