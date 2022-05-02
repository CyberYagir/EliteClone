using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMars : MonoBehaviour
{
    public Animator animator;

    public GameObject camera;


    public void StartMarsAnim()
    {
        animator.enabled = true;
        camera.GetComponent<Camera>().enabled = false;
        gameObject.SetActive(false);
    }
}
