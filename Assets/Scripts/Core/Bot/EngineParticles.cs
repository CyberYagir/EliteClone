    using UnityEngine;

namespace Core.Bot
{
    public class EngineParticles : MonoBehaviour
    {
        private Transform point;
        public static Transform holder;
        
        private void Start()
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


        private void Update()
        {
            if (point == null)
            {
                Destroy(gameObject);
                return;
            }
            transform.position = point.position;
        }
    
    }
}
