using System;
using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase.Intacts;
using TMPro;
using UnityEngine;

namespace Core.TDS.UI
{
    public class UseIndicator : MonoBehaviour
    {
        [SerializeField] private ShooterPlayerInteractor interactor;
        private TMP_Text text;

        private void Awake()
        {
            text = GetComponent<TMP_Text>();
            interactor.OnAddInter.AddListener(UpdateText);
            interactor.OnRemInter.AddListener(UpdateText);
            UpdateText();
        }

        public void UpdateText()
        {
            text.enabled = interactor.IsHaveInteractors();
        }
    }
}
