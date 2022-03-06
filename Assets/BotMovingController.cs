using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotMovingController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 currentRot;
    private float time;
    private void Start()
    {
        currentRot = -transform.position;
    }

    void FixedUpdate()
    {
        time += Time.deltaTime;
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);

        if (transform.position.magnitude > 1000 && time > 30)
        {
            currentRot = -transform.position;
            time = 0;
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(currentRot), 5 * Time.fixedDeltaTime);
    }
}
