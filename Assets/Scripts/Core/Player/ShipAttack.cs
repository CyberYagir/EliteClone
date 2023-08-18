using UnityEngine;

namespace Core.PlayerScripts
{
    public class ShipAttack : MonoBehaviour
    {
        public Event<int> OnShoot = new Event<int>();
        public Event<int> OnHold = new Event<int>();
        public Event<int> OnShootDown = new Event<int>();

    
        void LateUpdate()
        {
            var player = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
            if (player != null && !player.LandManager.isLanded)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (Input.GetKey(i.ToString()) || (i == 1 && Input.GetKey(KeyCode.Mouse0)) || (i == 2 && Input.GetKey(KeyCode.Mouse1)))
                    {
                        if (Input.GetKeyDown(i.ToString()) || (i == 1 && Input.GetKeyDown(KeyCode.Mouse0)) || (i == 2 && Input.GetKeyDown(KeyCode.Mouse1)))
                        {
                            OnShootDown.Run(i);
                        }
                        OnShoot.Run(i);

                    }
                    else
                    {
                        OnHold.Run(i);
                    }
                }
            }
        }
    }
}
