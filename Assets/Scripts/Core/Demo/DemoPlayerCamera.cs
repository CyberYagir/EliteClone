using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoPlayerCamera : MonoBehaviour
{
    [SerializeField] private Vector3 offcet;
    [SerializeField] private Transform player;
    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offcet, 10 * Time.fixedDeltaTime);
    }
}
