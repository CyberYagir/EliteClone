using System;
using TMPro;
using UnityEngine;

namespace Core.Garage
{
    public class ShipyardError : MonoBehaviour
    {
        public static ShipyardError Instance;
        [SerializeField] private GameObject error;

        private void Awake()
        {
            Instance = this;
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
