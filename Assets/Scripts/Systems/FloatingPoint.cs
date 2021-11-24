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
    private float time = 0;

    void Update()
    {
        time += Time.deltaTime;
        if (Player.inst && time > cooldown)
        {
            Vector3 cameraPos = Player.inst.transform.position;
            if (cameraPos.magnitude > threshold)
            {
                WorldSpaceObjectCanvas.canvas.UpdatePoints(true);
                foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    go.transform.position -= cameraPos;
                }
                WorldSpaceObjectCanvas.canvas.UpdatePoints(true);
            }

            time = 0;
        }
    }
}