using UnityEngine;
using UnityEngine.Rendering;

namespace Core
{
    public class InitEnvironment : MonoBehaviour
    {
        [SerializeField] private VolumeProfile profile;
        private void OnEnable()
        {
            FindObjectOfType<Volume>().profile = profile;
        }
    }
}
