using System;
using Core.Garage;
using Core.Map;
using Core.PlayerScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class MapUI : MonoBehaviour
    {
        [SerializeField] private GameObject fader;
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

        private void Update()
        {
            if (InputM.GetAxisDown(KAction.Map))
            {
                Player.inst.saves.SetKey("MapActive", (int) World.Scene);
                PlayerDataManager.SaveAll();
                var fd = Instantiate(fader.gameObject).GetComponent<FaderMultiScenes>();
                fd.LoadScene(Scenes.Map);
            }
        }
    }
}
