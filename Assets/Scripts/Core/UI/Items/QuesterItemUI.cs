using Core.Game;
using Core.Systems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
    public class QuesterItemUI : MonoBehaviour
    {
        private int fraction;
    
        [Space] 
        [SerializeField] private Image image;
        [SerializeField] private Image background;
        [SerializeField] private TMP_Text questerNameT, fractionNameT;
        private Character character;
        private int itemIndex;

        public void InitQuesterItem(int _fraction, Sprite _frationImage, string _questerName, Character _character, int itemID)
        {
            fraction = _fraction;
            image.sprite = _frationImage;

            questerNameT.text = _questerName;
            fractionNameT.text = WorldDataItem.Fractions.NameByID(fraction);

            character = _character;

            itemIndex = itemID;
        }

        public Character GetCharacter()
        {
            return character;
        }

        public void SetSelectedButton()
        {
            GetComponentInParent<CharacterList>().upDownUI.ForceChangeSelect(itemIndex);
        }
    }
}
