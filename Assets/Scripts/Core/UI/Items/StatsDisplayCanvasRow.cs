using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class StatsDisplayCanvasRow : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Transform value;


        public void SetValue(float _value, float maxValue, string _text = "_")
        {
            value.localScale = new Vector3(_value / maxValue, 1, 1);
            if (_text != "_")
            {
                text.text = _text;
            }
        }
    }
}
