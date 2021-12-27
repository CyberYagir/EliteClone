using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabineMesh : MonoBehaviour
{
    [SerializeField] private Vector3 point;
    private void Start()
    {
        PlayerHead.Instance.transform.localPosition = point;
    }

    private void OnDrawGizmos()
    {
        var ph = transform.root.GetComponentInChildren<PlayerHead>();
        var localPos = ph.transform.parent.InverseTransformPoint(point);
        Gizmos.DrawWireSphere(ph.transform.parent.TransformPoint(localPos), 0.02f);
    }
}
