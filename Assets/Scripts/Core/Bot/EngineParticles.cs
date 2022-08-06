    using UnityEngine;

namespace Core.Bot
{
    public class EngineParticles : MonoBehaviour
    {
        private Transform point;
        public static Transform holder;

        public void Init()
        {
            point = new GameObject("EnginePoint").transform;
            point.parent = transform.parent;
            point.localPosition = transform.localPosition;

            transform.parent = null;

            if (holder == null)
            {
                holder = new GameObject("EnginesHolder").transform;
            }

            transform.parent = holder;
        }

        public void UpdateObject()
        {
            transform.position = point.position;
        }
    }
}
