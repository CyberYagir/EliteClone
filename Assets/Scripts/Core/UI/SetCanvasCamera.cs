using UnityEngine;

namespace Core.UI
{
    public class SetCanvasCamera : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}
