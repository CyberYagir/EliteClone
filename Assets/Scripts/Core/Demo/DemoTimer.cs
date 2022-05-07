using TMPro;
using UnityEngine;

namespace Core.Core.Demo
{
    public class DemoTimer : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        private float time;
        private void LateUpdate()
        {
            time += Time.deltaTime;
            text.text = time.ToString();
        }
    }
}
