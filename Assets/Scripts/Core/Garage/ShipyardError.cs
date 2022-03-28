using TMPro;
using UnityEngine;

namespace Core.Garage
{
    public class ShipyardError : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        public void ThrowError(string message)
        {
            text.text = message;
            gameObject.SetActive(true);
        }
    }
}
