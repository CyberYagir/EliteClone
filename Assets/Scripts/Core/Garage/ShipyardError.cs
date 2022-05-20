using System;
using TMPro;
using UnityEngine;

namespace Core.Garage
{
    public class ShipyardError : Singleton<ShipyardError>
    {
        [SerializeField] private GameObject error;

        private void Awake()
        {
            Single(this);
            error.gameObject.SetActive(false);
        }

        [SerializeField] private TMP_Text text;
        public void ThrowError(string message)
        {
            text.text = message;
            error.gameObject.SetActive(true);
        }
    }
}
