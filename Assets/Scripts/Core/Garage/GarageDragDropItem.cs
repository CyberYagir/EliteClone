using Core.Game;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Garage
{
    public class GarageDragDropItem : Draggable
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameT, levelT;

        public void SetSprite(Item item)
        {
            image.sprite = item.icon;
            nameT.text = item.itemName;
            var level = (float) item.GetKeyPair(KeyPairValue.Level);
            if (level != 0f)
                levelT.text = "Level: " + (int)level;
            else
                levelT.gameObject.SetActive(false);
        }

        public override void StartDrag()
        {
            base.StartDrag();
            gameObject.SetActive(false);
        }

        public override void StopDrag()
        {
            base.StopDrag();
            gameObject.SetActive(true);
        }

        public override void Clicked()
        {
            base.Clicked();
            GetComponentInParent<GarageExplorer>().SetItem(data);
        }
    }
}
