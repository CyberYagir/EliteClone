using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GalaxyObject target { get; private set; }
    public List<ContactObject> contacts { get; private set; } = new List<ContactObject>();

    public Event ContactsChanges = new Event();

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        if (InputM.GetAxisDown(KAction.SetTarget))
        {
            if (target == null)
            {
                int id = -1;
                float angle = 9999;
                for (int i = 0; i < SolarSystemGenerator.objects.Count; i++)
                {
                    var ang = Vector3.Angle(SolarSystemGenerator.objects[i].transform.position - camera.transform.position, camera.transform.forward);
                    if (ang < angle)
                    {
                        angle = ang;
                        id = i;
                    }
                }

                if (angle < 5)
                {
                    SetTarget(SolarSystemGenerator.objects[id]);
                }
            }
            else
            {
                SetTarget(null);
            }
        }
    }

    public void SetTarget(GalaxyObject target)
    {
        this.target = target;
    }

    public void AddContact(ContactObject contact, bool trigger = true)
    {
        contacts.Add(contact);
        if (trigger)
            ContactsChanges.Run();
    }

    public void RemoveContact(ContactObject contact, bool trigger = true)
    {
        contacts.Remove(contact);
        if (trigger)
            ContactsChanges.Run();
    }
}
