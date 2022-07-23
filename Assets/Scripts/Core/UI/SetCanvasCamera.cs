using Core.PlayerScripts;
using UnityEngine;

namespace Core.UI
{
    public class SetCanvasCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            if (Player.inst != null)
            {
                GetComponent<Canvas>().worldCamera = Player.inst.GetCamera();
            }
        }
    }
}
