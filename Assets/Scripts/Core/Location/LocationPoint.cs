using System.Collections.Generic;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;

namespace Core.Location
{
    public class LocationPoint : MonoBehaviour
    {
        public enum LocationType
        {
            Station, Belt, Empty, Scene
        }
        private Camera mainCamera;
        
        [SerializeField] float size;
        [SerializeField] private GameObject root;
        [SerializeField] private float minDist;
        [SerializeField] private LocationType locationType;
        [HideInInspector]
        [SerializeField] private  Scenes scene;
        
        public GameObject Root => root;
        public LocationType Location => locationType;
        public Scenes Scene => scene;
        
        public Dictionary<string, object> data { get; private set; } = new Dictionary<string, object>();

        private void Start()
        {
            mainCamera = Player.inst.GetCamera();
        }
        void Update()
        {
            if (mainCamera != null)
            {
                transform.LookAt(mainCamera.transform);
                transform.localScale = Vector3.one * Vector3.Distance(transform.position, mainCamera.transform.position) * size;
                SetActiveLocation();
            }
        }

        public void SetData(Dictionary<string, object> newData)
        {
            data = newData;
        }
    
        public void SetActiveLocation()
        {
            if (Vector3.Distance(transform.position, mainCamera.transform.position) * SolarSystemGenerator.scale < minDist * SolarSystemGenerator.scale)
            {
                Player.inst.warp.SetActiveLocation(this);
            }
            else
            {
                Player.inst.warp.RemoveActiveLocation(this);
            }
        }

        public void SetScene(Scenes newScene)
        {
            scene = newScene;
        }
    }
}
