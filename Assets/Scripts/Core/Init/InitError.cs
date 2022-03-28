using TMPro;
using UnityEngine;

namespace Core.Init
{
    public class InitError : MonoBehaviour
    {
        [SerializeField] private CustomAnimate animate;
        [SerializeField] private TMP_Text text;
        private void Start()
        {
            if (PlayerPrefs.HasKey("Error"))
            {
                text.text = "Error: " + PlayerPrefs.GetString("Error");
                WindowManager.Instance.OpenWindow(animate);
                PlayerPrefs.DeleteKey("Error");
            }
        }

        public void Close()
        {
            animate.reverse = true;
        }
    }
}
