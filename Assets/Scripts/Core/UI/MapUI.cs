using Core.Map;
using Core.PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class MapUI : MonoBehaviour
    {
        private void Start()
        {
            Player.OnSceneChanged += Set;
            Set();
        }

        private void Set()
        {
            if (!MapGenerator.Set)
            {
                MapGenerator.Set = true;
                SceneManager.LoadScene("Map", LoadSceneMode.Additive);
            }
        }
    }
}
