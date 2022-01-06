using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldSpaceObjectCanvas : MonoBehaviour
{
    [SerializeField] private GameObject pointPrefab;
    private List<DisplaySpaceObject> spaceObjects = new List<DisplaySpaceObject>();
    
    public static WorldSpaceObjectCanvas Instance;

    
    private Camera camera;
    private bool skipFrame = false;
    [SerializeField] private GraphicRaycaster canvasRaycaster;
    public class DisplaySpaceObject
    {
        public WorldSpaceObject Obj;
        public WorldSpaceCanvasItem CanvasPoint;
        public bool isSystem;
    }


    private void Awake()
    {
        Instance = this;
        camera = Camera.main;
        Player.OnSceneChanged += UpdateList;
    }

    public void SkipFrame()
    {
        skipFrame = true;
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
        foreach (var wsp in SolarSystemGenerator.objects)
        {
            SpawnPoint(wsp);
        }

        foreach (var point in FindObjectsOfType<SolarSystemPoint>())
        {
            SpawnPoint(point.GetComponent<WorldSpaceObject>());
        }

        spaceObjects = spaceObjects.OrderBy(x => x.isSystem).ToList();
        UpdatePoints();
    }

    public void SpawnPoint(WorldSpaceObject wsp)
    {
        var obj = Instantiate(pointPrefab, transform).GetComponent<WorldSpaceCanvasItem>();
        spaceObjects.Add(new DisplaySpaceObject() {Obj = wsp, CanvasPoint = obj});
        obj.Init(wsp);
    }
    private void LateUpdate()
    {
        if (!skipFrame)
        {
            UpdatePoints();
        }
        else
        {
            skipFrame = false;
        }
    }
    public void UpdatePoints()
    {
        foreach (var wsp in spaceObjects)
        {
            if (wsp.Obj == null)
            {
                spaceObjects.RemoveAll(x=>x.Obj == null);
                return;
            }

            if (wsp.Obj.isVisible)
            {
                wsp.CanvasPoint.transform.position = (Vector2) camera.WorldToScreenPoint(wsp.Obj.transform.position, Camera.MonoOrStereoscopicEye.Mono);
                if (!wsp.isSystem)
                    wsp.CanvasPoint.transform.position = Raycast(wsp.CanvasPoint.transform.position);
                wsp.CanvasPoint.SetSelect(wsp.Obj == Player.inst.GetTarget());
            }

            wsp.CanvasPoint.gameObject.SetActive(wsp.Obj.isVisible);
        }
    }

    public Vector2 Raycast(Vector2 startPos)
    {
        var pointer = new PointerEventData(EventSystem.current);
        bool isEnded = false;
        int height = 35;
        int yOffcet = 0;
        List<RaycastResult> results = new List<RaycastResult>(1);
        while (!isEnded)
        {
            results = new List<RaycastResult>(1);
            
            pointer.position = startPos + new Vector2(0, yOffcet);
            canvasRaycaster.Raycast(pointer, results);

            if (results.Count == 0)
            {
                break;
            }
            else
            {
                yOffcet += height;
            }
        }
        
        return startPos + new Vector2(0, yOffcet);
    }
}
