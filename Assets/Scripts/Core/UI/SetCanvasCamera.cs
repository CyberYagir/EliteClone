using Core.PlayerScripts;
using UnityEngine;

namespace Core.UI
{
    public class SetCanvasCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var player = PlayerDataManager.Instance.WorldHandler.ShipPlayer;
            if (player != null)
            {
                GetComponent<Canvas>().worldCamera = player.GetCamera();
            }
        }
    }
}
