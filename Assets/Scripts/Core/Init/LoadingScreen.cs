using TMPro;
using UnityEngine;

namespace Core.Init
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
    
        private void Update()
        {
            text.text = (PlayerDataManager.GenerateProgress * 100).ToString("F1") + "%";
        }
    }
}
