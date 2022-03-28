using System.Collections;
using UnityEngine;

namespace Core.Garage
{
    public class UIGarageMover : MonoBehaviour
    {
        [SerializeField] private GameObject fader;
    
        public void LoadLocation()
        {
            Player.Player.inst.saves.Save();
            var fd = Instantiate(fader).GetComponent<Fader>();
            fd.SetColor(0);
            fd.Fade(1);
            StartCoroutine(WaitForFader());
        }

        IEnumerator WaitForFader()
        {
            yield return new WaitForSeconds(1);
            World.LoadLevelAsync(Scenes.Garage);
        }
    }
}
