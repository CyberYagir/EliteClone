using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] private Image fractionImage;
        [SerializeField] private TMP_Text fractionName;
        [SerializeField] private int itemIndex;

        public void Init(Sprite fractionIcon, string fname, int itemID)
        {
            fractionImage.sprite = fractionIcon;
            fractionImage.enabled = true;
            fractionName.text = fname;
            fractionName.enabled = true;

            itemIndex = itemID;
        }

        public void SelectItem()
        {
            GetComponentInParent<QuestListUI>().upDownUI.ForceChangeSelect(itemIndex);
        }
    }
}
