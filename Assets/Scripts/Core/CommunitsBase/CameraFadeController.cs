using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class CameraFadeController : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private LayerMask mask;

        [SerializeField] private List<AlphaController> controllers = new List<AlphaController>();
        void FixedUpdate()
        {
            var inThisFrame = new List<AlphaController>();
            if (Physics.Raycast(transform.position, player.position - transform.position, out RaycastHit hit, Mathf.Infinity,mask))
            {
                if (hit.collider != null)
                {
                    if (hit.transform.GetComponentInParent<ShooterPlayer>() == null)
                    {
                        var alpha = hit.collider.GetComponentInParent<AlphaController>();

                        if (alpha)
                        {
                            inThisFrame.Add(alpha);
                            if (!controllers.Contains(alpha))
                            {
                                controllers.Add(alpha);
                                alpha.StartFade();
                            }
                        }
                    }
                }
            }
            
            for (int i = 0; i < controllers.Count; i++)
            {
                if (!inThisFrame.Contains(controllers[i]))
                {
                    controllers[i].DisableFade();
                    controllers.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
