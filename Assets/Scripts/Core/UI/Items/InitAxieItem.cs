using TMPro;
using UnityEngine;

namespace Core.Init
{
    public class InitAxieItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text axieName;
        [SerializeField] private TMP_Text plus, minus;

        private InputService.Axis current;
        public void Init(InputService.Axis axis)
        {
            current = axis;
            axieName.text = axis.name;

            plus.transform.parent.gameObject.SetActive(axis.plus != KeyCode.None);
            if (axis.plus != KeyCode.None)
            {
                plus.text = axis.plus.ToString();
            }

        
            minus.transform.parent.gameObject.SetActive(axis.minus != KeyCode.None);
            if (axis.minus != KeyCode.None)
            {
                minus.text = axis.minus.ToString();
            }
        }

        public void Change(bool plus)
        {
            GetComponentInParent<InitOptionsControlsDrawer>().ChangeAxis(current, plus);
            if (plus)
            {
                this.plus.text = "Press key";
            }
            else
            {
                minus.text = "Press key";
            }
        }
    }
}
