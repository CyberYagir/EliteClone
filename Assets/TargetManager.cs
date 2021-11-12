using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public WorldSpaceObject target { get; private set; }
    public List<ContactObject> contacts { get; private set; }

    public void SetTarget(WorldSpaceObject target)
    {
        if (target != null)
        {
            this.target = target;
        }
    }
}
