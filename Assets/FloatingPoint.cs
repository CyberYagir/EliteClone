using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingPoint : MonoBehaviour
{

    public float threshold = 10000.0f;

    void LateUpdate()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        cameraPos.y = 0f;
        if (cameraPos.magnitude > threshold)
        {
            foreach (GameObject go in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                go.transform.position -= cameraPos;
            }
        }
    }
}