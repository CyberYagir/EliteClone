using TMPro;
using UnityEngine;

namespace Core.Garage
{
    public class ShiyardCreditsDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private void Start()
        {
            var yard = FindObjectOfType<Shipyard>();
            yard.OnChange += UpdateText;
        }

        public void UpdateText()
        {
            text.text = "Credits have: " + GarageDataCollect.Instance.cargo.GetCredits();
        }
    }
}
