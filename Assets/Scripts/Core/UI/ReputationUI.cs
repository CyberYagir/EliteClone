using System;
using System.Collections;
using System.Collections.Generic;
using Core.Player;
using Core.UI;
using UnityEngine;

namespace Core.UI
{
    public class ReputationUI : MonoBehaviour
    {
        [SerializeField] private GameObject item;
        [SerializeField] private Transform holder;

        private void Start()
        {
            Redraw();
            ReputationManager.Instance.OnChangeRating += Redraw;
        }

        public void Redraw()
        {
            UITweaks.ClearHolder(holder);
            var max = ReputationManager.Instance.GetMax();
            foreach (var rn in ReputationManager.Instance.ratings)
            {
                var it = Instantiate(item.gameObject, holder.transform).GetComponent<ReputationItemUI>();
                it.Init(rn.Value, rn.Key, max);
            }
        }
    }
}
