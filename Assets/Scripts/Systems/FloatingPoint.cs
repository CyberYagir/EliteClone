using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingPoint : MonoBehaviour
{

    public float threshold = 10000.0f;

    private float cooldown = 2;

    void Update()
    {
        if (Player.inst)
        {
            Vector3 cameraPos = Player.inst.transform.position;
            if (cameraPos.magnitude > threshold)
            {
                foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    go.transform.position -= cameraPos;
                }
                foreach (var wsp in SolarSystemGenerator.objects)
                {
                    wsp.UpdateVisibility();
                }
                WorldSpaceObjectCanvas.Instance.SkipFrame();
            }

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -100000, 100000),
                Mathf.Clamp(transform.position.y, -100000, 100000), Mathf.Clamp(transform.position.z, -100000, 100000));
        }
    }
}