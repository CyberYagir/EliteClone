using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotBuilder : MonoBehaviour
{
    [SerializeField] private BotAttackController attackControl;
    [SerializeField] private BotLandController landControl;
    [SerializeField] private ShieldActivator shield;
    [SerializeField] private BotVisual visual;
    [SerializeField] private ContactObject contactManager;
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

    public void InitBot(bool triggerEvent, System.Random rnd = null)
    {
        NamesHolder.Init();
        var firstName = "";
        var lastName = "";
        if (rnd == null)
        {
            firstName = NamesHolder.ToUpperFist(NamesHolder.Instance.FirstNames[UnityEngine.Random.Range(0, NamesHolder.Instance.FirstNames.Length)]);
            lastName = NamesHolder.ToUpperFist(NamesHolder.Instance.LastNames[UnityEngine.Random.Range(0, NamesHolder.Instance.LastNames.Length)]);
        }
        else
        {
            firstName = NamesHolder.ToUpperFist(NamesHolder.GetFirstName(rnd));
            lastName = NamesHolder.ToUpperFist(NamesHolder.GetLastName(rnd));
        }

        transform.name = GetComponent<BotVisual>().GetShipName() + $" [{firstName} {lastName}]";
    }

    public ParticleSystem PlayWarp()
    {
        particles.Play();
        return particles;
    }

    public ShieldActivator GetShield()
    {
        return shield;
    }

    public BotVisual GetVisual()
    {
        return visual;
    }

    public void AddContact(bool trigger)
    {
        contactManager.Init(trigger);
    }
}
