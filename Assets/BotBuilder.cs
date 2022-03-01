using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotBuilder : MonoBehaviour
{
    [SerializeField] private BotAttackController attackControl;
    [SerializeField] private BotLandController landControl;
    [SerializeField] private ParticleSystem particles;
    private void Start()
    {
        if (World.Scene == Scenes.Location)
        {
            attackControl.enabled = false;
            landControl.enabled = true;
        }
        else
        {
            attackControl.enabled = true;
            landControl.enabled = false;
        }
    }

    public ParticleSystem PlayWarp()
    {
        particles.Play();
        return particles;
    }
}
