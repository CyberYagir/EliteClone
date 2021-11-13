using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHologram : MonoBehaviour
{
    [SerializeField] List<Renderer> meshes;
    float alpha;
    private void Update()
    {
        if (Player.inst.GetTarget() == null)
        {
            alpha -= Time.deltaTime;
        }
        else
        {
            alpha += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Player.inst.GetTarget().transform.position - transform.position, Vector3.up), 10 * Time.deltaTime);
        }

        alpha = Mathf.Clamp(alpha, 0, 0.2f);
        for (int i = 0; i < meshes.Count; i++)
        {
            meshes[i].material.color = new Color(meshes[i].material.color.r, meshes[i].material.color.g, meshes[i].material.color.b, alpha);
        }
    }

}
