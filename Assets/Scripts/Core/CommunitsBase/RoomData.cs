using UnityEngine;

namespace Core.CommunistsBase
{
    [System.Serializable]
    public class RoomData
    {
        private MeshRenderer[] renderers;
        private Light[] lights;
        public Transform Obj { get; private set; }
        public AlphaController controller { get; private set; }

        [SerializeField] private bool stateActive = true;

        public RoomData(Transform holder)
        {
            Obj = holder;
            controller = Obj.GetComponent<AlphaController>();
            renderers = holder.GetComponentsInChildren<MeshRenderer>();
            lights = holder.GetComponentsInChildren<Light>();
        }


        public bool IsOverride()
        {
            if (controller == null)
            {
                return false;
            }
            else
            {
                return controller.overrideActive;
            }
        }

        public void Active(bool state)
        {
            stateActive = state;
            foreach (var n in renderers)
            {
                n.enabled = state;
            }

            foreach (var l in lights)
            {
                l.enabled = state;
            }
        }
    }
}