using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FloatingPoint : MonoBehaviour
{

    public float threshold = 10000.0f;

    void LateUpdate()
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
            }
        }
    }
}