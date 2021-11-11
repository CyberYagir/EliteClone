using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldSpaceObjectCanvas : MonoBehaviour
{
    List<DisplaySpaceObject> spaceObjects = new List<DisplaySpaceObject>();
    [SerializeField] GameObject pointPrefab;

    Camera camera;
    public class DisplaySpaceObject
    {
        public WorldSpaceObject obj;
        public Transform canvasPoint;
    }


    private void Start()
    {
        camera = Camera.main;
        foreach (var wsp in SolarSystemGenerator.objects)
        {
            spaceObjects.Add(new DisplaySpaceObject() { obj = wsp, canvasPoint = Instantiate(pointPrefab, transform).transform });
        }
    }

    private void LateUpdate()
    {
        UpdatePoints();
    }

    public void UpdatePoints()
    {
        foreach (var wsp in spaceObjects)
        {
            if (wsp.obj.isVisible)
            {
                wsp.canvasPoint.transform.position = Vector2.Lerp(wsp.canvasPoint.transform.position, camera.WorldToScreenPoint(wsp.obj.transform.position, Camera.MonoOrStereoscopicEye.Mono), Screen.width * 0.02f * Time.deltaTime);
                wsp.canvasPoint.transform.localPosition = new Vector3(wsp.canvasPoint.transform.localPosition.x, wsp.canvasPoint.transform.localPosition.y, 0);
                var texts = wsp.canvasPoint.GetComponentsInChildren<TMP_Text>();
                texts[0].text = wsp.obj.transform.name;
                if (Player.inst.control.speed != 0)
                {
                    texts[1].text = ((Vector2.Distance(camera.transform.position, wsp.obj.transform.position) / Mathf.Abs(Player.inst.control.speed)) / 60f).ToString("F2") + " ~min";
                }
                else
                {
                    texts[1].text = ">100y";
                }
            }
            wsp.canvasPoint.gameObject.SetActive(wsp.obj.isVisible);
        }
    }
}
