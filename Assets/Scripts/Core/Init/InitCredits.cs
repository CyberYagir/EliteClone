using TMPro;
using UnityEngine;

namespace Core.Init
{
    public class InitCredits : MonoBehaviour
    {
        [SerializeField] private TMP_Text version;

        private void Start()
        {
            version.text = "v" + Application.version;
        }

        public void OpenURL(string url)
        {
            Application.OpenURL(url);
        }
    }
}
