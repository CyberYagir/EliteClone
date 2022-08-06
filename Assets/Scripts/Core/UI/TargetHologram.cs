using System;
using System.Collections;
using System.Collections.Generic;
using Core.PlayerScripts;
using Core.Systems;
using UnityEngine;

namespace Core.UI
{
    public class TargetHologram : MonoUI
    {
        [SerializeField] private List<Renderer> meshes;
        [SerializeField] private Transform arrow;
        private float alpha = 1;

        private void Start()
        {
            StartCoroutine(Transition());
        }

        public override void OnUpdate()
        {
            if (Player.inst.GetTarget() != oldTarget)
            {   
                StopAllCoroutines();
                StartCoroutine(Transition());
                oldTarget = Player.inst.GetTarget();
            }
        }

        private GalaxyObject oldTarget = null;

        IEnumerator Transition()
        {
            while (Player.inst.GetTarget() != null ? alpha < 1 : alpha > 0)
            {
                if (Player.inst.GetTarget() == null)
                {
                    alpha -= Time.deltaTime;
                }
                else
                {
                    alpha += Time.deltaTime;
                    arrow.rotation = Quaternion.Lerp(arrow.rotation, Quaternion.LookRotation(Player.inst.GetTarget().transform.position - transform.position, Vector3.up), 10 * Time.deltaTime);
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

                yield return null;
            }
        }
    }
}
