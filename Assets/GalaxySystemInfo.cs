using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        massT.text = "M:" + s.solarSystem.stars[0].mass.ToString("F5");
        radiusT.text = "R: " + s.solarSystem.stars[0].radius.ToString("F5");
        posXT.text = "X: " + s.solarSystem.position.x.ToString("F5");
        posYT.text = "Y: " + s.solarSystem.position.y.ToString("F5");
        posZT.text = "Z: " + s.solarSystem.position.z.ToString("F5");
    }
}
