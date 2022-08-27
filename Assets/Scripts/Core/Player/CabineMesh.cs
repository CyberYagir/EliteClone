using UnityEngine;

namespace Core.PlayerScripts
{
    public class CabineMesh : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            var ph = transform.root.GetComponentInChildren<PlayerHead>();
            if (ph != null)
            {
                var localPos = ph.transform.parent.InverseTransformPoint(Vector3.zero);
                Gizmos.DrawWireSphere(ph.transform.parent.TransformPoint(localPos), 0.01f);
            }
        }
    }
}
