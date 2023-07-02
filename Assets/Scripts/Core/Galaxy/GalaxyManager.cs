using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.Galaxy
{
    public class GalaxyManager : MonoBehaviour
    {
        public static GalaxyPoint selectedPoint { get; private set; }
        public static Event onUpdateSelected = new Event();
        private static WorldDataHandler worldHandler;
        
        
        private void Awake()
        {
            onUpdateSelected = new Event();
        }

        private void Update()
        {
            if (selectedPoint != null)
            {
                selectedPoint.particles.Play();
            }
        }



        public static void InitStaticBuilder(WorldDataHandler handler)
        {
            worldHandler = handler;
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
                    onUpdateSelected.Run();
                    return true;
                }
            }

            return false;
        }


        public static void JumpToSolarSystem()
        {
            worldHandler.ChangeSolarSystem(selectedPoint.solarSystem);
            World.LoadLevel(Scenes.System);
        }


    }
}
