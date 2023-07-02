using System.Collections;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.Garage
{
    public class UIGarageMover : MonoBehaviour
    {
        [SerializeField] private GameObject fader;

        [SerializeField] private Scenes scene = Scenes.Garage;
        
        public void LoadLocation()
        {
            PlayerDataManager.Instance.WorldHandler.ShipPlayer.saves.Save();
            var fd = Instantiate(fader).GetComponent<Fader>();
            fd.SetColor(0);
            fd.Fade();
            StartCoroutine(WaitForFader());
        }

        IEnumerator WaitForFader()
        {
            yield return new WaitForSeconds(1);
            World.LoadLevelAsync(scene);
        }
    }
}
