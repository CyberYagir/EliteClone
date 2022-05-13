using Core.Systems;
using Core.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Galaxy
{
    public class GalaxySystemInfo : MonoBehaviour
    {
        [SerializeField] RawImage preview;
        [SerializeField] TMP_Text nameT, typeT, massT, radiusT, posXT, posYT, posZT;



        private void Start()
        {
            GalaxyManager.onUpdateSelected += UpdateData;
            GalaxyManager.onUpdateSelected += delegate { GetComponent<ActiveWindow>().SetOpenClose(true); };
        }

        private void UpdateData()
        {
            var s = GalaxyManager.selectedPoint;
            preview.texture = s.GetTexture();
            preview.color = s.particles.startColor;
            nameT.text = s.solarSystem.stars[0].name;
            typeT.text = s.solarSystem.stars[0].starType.ToString();
            if (SolarSystemGenerator.GetStarsCount(s.solarSystem.position) == 1)
            {
                if (s.solarSystem.stars[0].starType != Star.StarType.Hole && new System.Random((int) GalaxyManager.selectedPoint.solarSystem.position.ToVector().magnitude * 10000).Next(0, 5) == 1)
                {
                    typeT.text = "Dyson";
                }
            }

            massT.text = "M:" + s.solarSystem.stars[0].mass.ToString("F5");
            radiusT.text = "R: " + s.solarSystem.stars[0].radius.ToString("F5");
            posXT.text = "X: " + s.solarSystem.position.x.ToString("F5");
            posYT.text = "Y: " + s.solarSystem.position.y.ToString("F5");
            posZT.text = "Z: " + s.solarSystem.position.z.ToString("F5");
        }
    }
}
