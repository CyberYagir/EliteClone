using Core.PlayerScripts;
using Core.UI;
using TMPro;
using UnityEngine;

namespace Core
{
    public class ItemUIDrop : MonoBehaviour
    {
        [SerializeField] private ItemUI itemUI;
        [SerializeField] private TMP_Text text;
        private float val, valSpeedAdd;
        private float speed;
        public bool isDrop;
        private float timer;
        public void ResetText()
        {
            isDrop = false;
            text.text = "";
            val = 1;
        }

        public void StartEdit()
        {
            isDrop = true;
        }


        public void EditVal()
        {
            timer += Time.deltaTime;
            if (InputM.GetAxisDown(KAction.TabsHorizontal))
            {
                speed = 0.15f;
                valSpeedAdd = 0;
            }
            if (timer >= speed)
            {
                var axis = InputM.GetAxisRaw(KAction.TabsHorizontal);
                val += InputM.GetAxisRaw(KAction.TabsHorizontal) + (valSpeedAdd * axis);
                val = Mathf.Clamp(val, 1f, itemUI.item.amount.value);
                text.text = "-" + val;
                speed -= Time.deltaTime / 2f;
                if (speed <= 0)
                {
                    speed = 0.006f;
                    valSpeedAdd++;
                }
                timer = 0;
            } 
        }

        public void DropItem()
        {
            Player.inst.cargo.DropItem(itemUI.item.id.id, itemUI.item.amount.value, val);
        }
    }
}
