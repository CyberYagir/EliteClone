using Core.PlayerScripts;
using UnityEngine;

namespace Core.UI
{
    public class LocationUIActivator : MonoBehaviour
    {
        [SerializeField] private GameObject baseUI;
        void Start()
        {
            Player.OnSceneChanged += OnAddEvent;
        }

        // private void OnDestroy()
        // {
        //     Player.OnSceneChanged -= OnAddEvent;
        // }

        public void OnAddEvent()
        {
            baseUI.SetActive(World.Scene == Scenes.Location);
        }
    
    }
}
                                                                                                                        