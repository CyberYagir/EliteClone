using Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class WorldSpaceCanvasItem : MonoBehaviour
    {
        [SerializeField] private Image image, selection;
        [SerializeField] private TMP_Text text;

        public void Init(WorldSpaceObject wso)
        {
            image.sprite = wso.icon;
            text.text = wso.transform.name;
        }

        public void SetText(string str)
        {
            if (text.text != str)
            {
                text.text = str;
            }
        }

        public void SetSelect(bool state)
        {
            if (state != gameObject.active)
                selection.gameObject.SetActive(state);
        }
    }
}
