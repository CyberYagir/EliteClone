using System.Collections.Generic;
using UnityEngine;

namespace Core.UI
{
    public class TargetHologram : MonoBehaviour
    {
        [SerializeField] private List<Renderer> meshes;
        [SerializeField] private Transform arrow;
        private float alpha;
        private void Update()
        {
            if (Player.Player.inst.GetTarget() == null)
            {
                alpha -= Time.deltaTime;
            }
            else
            {
                alpha += Time.deltaTime;
                arrow.rotation = Quaternion.Lerp(arrow.rotation, Quaternion.LookRotation(Player.Player.inst.GetTarget().transform.position - transform.position, Vector3.up), 10 * Time.deltaTime);
            }

            alpha = Mathf.Clamp(alpha, 0, 0.2f);
            for (int i = 0; i < meshes.Count; i++)
            {
                meshes[i].material.color = new Color(meshes[i].material.color.r, meshes[i].material.color.g, meshes[i].material.color.b, alpha);
                if (alpha > 0)
                {
                    if (!meshes[i].gameObject.active)
                        meshes[i].gameObject.SetActive(true);
                }
                else
                {
                    if (meshes[i].gameObject.active)
                        meshes[i].gameObject.SetActive(false);
                }
            }
        }

    }
}
