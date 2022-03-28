using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesFromSpeed : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        particleSystem.startSpeed = Player.inst.control.speed * 10;
        var rm = particleSystem.emission;
        rm.rateOverTime = Player.inst.control.speed * 10;

        var f = particleSystem.forceOverLifetime;
        f.x = Mathf.Sign(Player.inst.control.yaw) * -20 * (Player.inst.control.yaw == 0 ? 0 : 1);
        f.y = Player.inst.control.vertical * -20;
    }
}
