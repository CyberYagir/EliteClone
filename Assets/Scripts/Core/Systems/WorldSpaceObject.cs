using Core.PlayerScripts;
using UnityEngine;

namespace Core.Systems
{
    public abstract class GalaxyObject : MonoBehaviour
    {
        public Sprite icon;
    }

    public class WorldSpaceObject : GalaxyObject
    {
        public bool isVisible;    
        [HideInInspector]
        public string dist;
        private Transform camera;
        private void Start()
        {
            camera = Player.inst.GetCamera().transform;
        }
        

        public void UpdateVisibility()
        {
            if (camera)
            {
                isVisible = Vector3.Angle(transform.position - camera.transform.position, camera.forward) < 60;
            }
        }

        private void OnDestroy()
        {
            if (SolarSystemGenerator.objects != null)
            {
                SolarSystemGenerator.objects.Remove(this);
            }
        }
    }
}