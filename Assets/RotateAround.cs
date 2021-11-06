using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Vector3 point;

    public float speed;
    private void Start()
    {
        speed = new System.Random((int)transform.position.magnitude).Next(1, 20);
    }

    void Update()
    {
        transform.RotateAround(point, Vector3.up, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(point, 10);
    }
}
