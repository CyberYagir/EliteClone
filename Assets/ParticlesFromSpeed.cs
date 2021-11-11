using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFromSpeed : MonoBehaviour
{
    ParticleSystem particleSystem;
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        particleSystem.startSpeed = Player.inst.control.speed * 10;
        var rm = particleSystem.emission;
        rm.rateOverTime = Player.inst.control.speed * 10;
    }
}
