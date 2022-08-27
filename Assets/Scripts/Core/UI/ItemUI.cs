using Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class ItemUI : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text text, val; 
        public Item item;

        public void Init(Item it)
        {
            item = it;

            image.sprite = it.icon;
            if (val == null)
            {
                if (it.IsHaveKeyPair(KeyPairValue.Mineral))
                {
                    text.text = it.itemName + $" [{(int) ((it.amount.Value / it.amount.Max) * 100)}%]";
                }
                else
                {
                    text.text = it.itemName + $" [{it.amount.Value}]";
                }
            }
            else
            {
                text.text = it.itemName;
                
                
                if (it.IsHaveKeyPair(KeyPairValue.Mineral))
                {
                    val.text = $" [{(int) ((it.amount.Value / it.amount.Max) * 100)}%]";
                }
                else
                {
                    val.text = $" [{it.amount.Value}]";
                }
            }
        }
    }
}
