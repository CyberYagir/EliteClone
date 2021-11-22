using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldSpaceObjectCanvas : MonoBehaviour
{
    List<DisplaySpaceObject> spaceObjects = new List<DisplaySpaceObject>();
    [SerializeField] GameObject pointPrefab;
    public static WorldSpaceObjectCanvas canvas;
    Camera camera;
    public class DisplaySpaceObject
    {
        public WorldSpaceObject obj;
        public Transform canvasPoint;
    }


    private void Awake()
    {
        canvas = this;
        camera = Camera.main;
        Player.OnSceneChanged += UpdateList;
    }
    private void OnDestroy()
    {
        Player.OnSceneChanged -= UpdateList;
    }
    public void UpdateList()
    {
        spaceObjects = new List<DisplaySpaceObject>();
        foreach (Transform tr in transform)
        {
            if (tr.gameObject.active)
            {
                Destroy(tr.gameObject);
            }
        }
        foreach (var wsp in FindObjectsOfType<WorldSpaceObject>())
        {
            spaceObjects.Add(new DisplaySpaceObject() { obj = wsp, canvasPoint = Instantiate(pointPrefab, transform).transform });
        }

        UpdatePoints(true);
    }
    private void Update()
    {
        UpdatePoints();
    }

    public void UpdatePoints(bool withoutLerp = false)
    {
        foreach (var wsp in spaceObjects)
        {
            if (wsp.obj == null)
            {
                spaceObjects.Remove(wsp);
                return;
            }

            if (wsp.obj.isVisible)
            {
                Physics.Raycast(camera.transform.position, wsp.obj.transform.position - camera.transform.position,
                    out RaycastHit hit);
                var old = withoutLerp;
                if (wsp.obj.isVisible == true && wsp.canvasPoint.gameObject.active == false)
                {
                    withoutLerp = true;
                }

                if (withoutLerp)
                {
                    wsp.canvasPoint.transform.position =
                        camera.WorldToScreenPoint(wsp.obj.transform.position, Camera.MonoOrStereoscopicEye.Mono);
                }
                else
                {
                    var point = camera.WorldToScreenPoint(wsp.obj.transform.position,
                        Camera.MonoOrStereoscopicEye.Mono);
                    point = new Vector3(point.x, point.y, 0);
                    if (!Dist(wsp.canvasPoint.transform, wsp.obj.transform))
                    { 
                        wsp.canvasPoint.transform.position =
                            point;
                        wsp.canvasPoint.gameObject.SetActive(false);
                    }
                    else
                    {
                        
                        wsp.canvasPoint.transform.position =
                            Vector3.Lerp(wsp.canvasPoint.transform.position,
                                point, Screen.width * 0.5f * Time.deltaTime);
                    }
                }

                withoutLerp = old;

                wsp.canvasPoint.GetChild(0).gameObject.SetActive(Player.inst.GetTarget() == wsp.obj);
                wsp.canvasPoint.transform.localPosition = new Vector3(wsp.canvasPoint.transform.localPosition.x,
                    wsp.canvasPoint.transform.localPosition.y, 0);
                var texts = wsp.canvasPoint.GetComponentsInChildren<TMP_Text>();
                texts[0].text = wsp.obj.transform.name;
                if (Player.inst.control.speed != 0)
                {
                    wsp.obj.dist =
                        ((Vector2.Distance(camera.transform.position, hit.point) /
                          Mathf.Abs(Player.inst.control.speed + Player.inst.warp.warpSpeed)) / 60f).ToString("F2") +
                        " ~min";
                }
                else
                {
                    wsp.obj.dist = ">100y";
                }

                texts[1].text = wsp.obj.dist;
            }

            if (Dist(wsp.canvasPoint.transform, wsp.obj.transform))
                wsp.canvasPoint.gameObject.SetActive(wsp.obj.isVisible);
        }
    }

    public bool Dist(Transform canvasPoint, Transform worldObject)
    {
        return Vector2.Distance(canvasPoint.position,
                camera.WorldToScreenPoint(worldObject.position, Camera.MonoOrStereoscopicEye.Mono)) < Screen.width * 0.2f;
    }
}
