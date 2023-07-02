using Core.PlayerScripts;
using UnityEngine;

namespace Core.Systems
{
    public class PosToPlayerPos : MonoBehaviour
    {
        public void Update()
        {
            var player = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
            if (player)
            {
                transform.position = player.transform.position;
            }
        }
    }
}