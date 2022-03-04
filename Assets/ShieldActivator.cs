using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ShieldActivator : MonoBehaviour
{
    [SerializeField] private Transform shield;

    private bool active;

    public bool isActive
    {
        set
        {
            active = value;
            shield.DOScale(Vector3.one, 0.5f);
        }
        get
        {
            shield.DOScale(Vector3.zero, 0.5f);
            return active;
        }
    }
}
