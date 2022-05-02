using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoShoot : MonoBehaviour
{
    [SerializeField] private GameObject particles;


    public void EnableParticles()
    {
        particles.gameObject.SetActive(true);
    }
}
