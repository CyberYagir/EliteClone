using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GalaxyManager : MonoBehaviour
{
    public static GalaxyPoint selectedPoint { get; private set; }
    public static event Action onUpdateSelected = delegate { };

    private void Awake()
    {
        onUpdateSelected = delegate { };
    }

    private void Start()
    {
        Application.targetFrameRate = 120;
    }

    private void Update()
    {
        if (selectedPoint != null)
        {
            selectedPoint.particles.Play();
        }
    }

    

    public static bool Select(GalaxyPoint newSel)
    {
        var m_PointerEventData = new PointerEventData(EventSystem.current);
        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        FindObjectOfType<GraphicRaycaster>().Raycast(m_PointerEventData, results);

        if (results.Count == 0)
        {
            if (newSel != selectedPoint)
            {
                selectedPoint = newSel;
                onUpdateSelected();
                return true;
            }
        }

        return false;
    }


    public static void JumpToSolarSystem()
    {
        PlayerDataManager.currentSolarSystem = selectedPoint.solarSystem;
        Application.LoadLevel("System");
    }


}
