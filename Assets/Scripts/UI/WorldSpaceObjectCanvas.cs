using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorldSpaceObjectCanvas : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    private List<DisplaySpaceObject> spaceObjects = new List<DisplaySpaceObject>();
    
    public static WorldSpaceObjectCanvas Instance;

    private Camera camera;
    
    public class DisplaySpaceObject
    {
        public WorldSpaceObject Obj;
        public Transform CanvasPoint;
    }


    private void Awake()
    {
        Instance = this;
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
            spaceObjects.Add(new DisplaySpaceObject() { Obj = wsp, CanvasPoint = Instantiate(pointPrefab, transform).transform });
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
            if (wsp.Obj == null)
            {
                spaceObjects.Remove(wsp);
                return;
            }

            if (wsp.Obj.isVisible)
            {
                Physics.Raycast(camera.transform.position, wsp.Obj.transform.position - camera.transform.position,
                    out RaycastHit hit);
                var old = withoutLerp;
                if (wsp.Obj.isVisible == true && wsp.CanvasPoint.gameObject.active == false)
                {
                    withoutLerp = true;
                }

                if (withoutLerp)
                {
                    wsp.CanvasPoint.transform.position =
                        camera.WorldToScreenPoint(wsp.Obj.transform.position, Camera.MonoOrStereoscopicEye.Mono);
                }
                else
                {
                    var point = camera.WorldToScreenPoint(wsp.Obj.transform.position,
                        Camera.MonoOrStereoscopicEye.Mono);
                    point = new Vector3(point.x, point.y, 0);
                    if (!Dist(wsp.CanvasPoint.transform, wsp.Obj.transform))
                    { 
                        wsp.CanvasPoint.transform.position =
                            point;
                        wsp.CanvasPoint.gameObject.SetActive(false);
                    }
                    else
                    {
                        
                        wsp.CanvasPoint.transform.position =
                            Vector3.Lerp(wsp.CanvasPoint.transform.position,
                                point, Screen.width * 0.5f * Time.deltaTime);
                    }
                }

                withoutLerp = old;

                wsp.CanvasPoint.GetChild(0).gameObject.SetActive(Player.inst.GetTarget() == wsp.Obj);
                wsp.CanvasPoint.transform.localPosition = new Vector3(wsp.CanvasPoint.transform.localPosition.x,
                    wsp.CanvasPoint.transform.localPosition.y, 0);
                var texts = wsp.CanvasPoint.GetComponentsInChildren<TMP_Text>();
                texts[0].text = wsp.Obj.transform.name;
                if (Player.inst.control.speed != 0)
                {
                    wsp.Obj.dist =
                        ((Vector2.Distance(camera.transform.position, hit.point) /
                          Mathf.Abs(Player.inst.control.speed + Player.inst.warp.warpSpeed)) / 60f).ToString("F2") +
                        " ~min";
                }
                else
                {
                    wsp.Obj.dist = ">100y";
                }

                texts[1].text = wsp.Obj.dist;
            }

            if (Dist(wsp.CanvasPoint.transform, wsp.Obj.transform))
                wsp.CanvasPoint.gameObject.SetActive(wsp.Obj.isVisible);
        }
    }

    public bool Dist(Transform canvasPoint, Transform worldObject)
    {
        return Vector2.Distance(canvasPoint.position,
                camera.WorldToScreenPoint(worldObject.position, Camera.MonoOrStereoscopicEye.Mono)) < Screen.width * 0.2f;
    }
}
