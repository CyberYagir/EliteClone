using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMeshSlotRotate : MonoBehaviour
{
    private Quaternion startRotation;
    private Weapon weapon;
    private void Awake()
    {
        startRotation = transform.localRotation;
    }

    private void Start()
    {
        weapon = GetComponent<Weapon>();
    }

    private void LateUpdate()
    {
        if (Player.inst)
        {
            var target = Player.inst.GetTarget();
            if (target && Vector3.Distance(transform.position, target.transform.position) < weapon.GetDistance())
            {
                Quaternion targetRotation = Quaternion.identity;
                var dir = target.transform.position - Player.inst.transform.position;
                if (Vector3.Dot(Player.inst.transform.forward, dir) > 0)
                {
                    var ang = Vector3.Angle(Player.inst.transform.forward, dir);
                    if (ang < 30)
                    {
                        targetRotation = Quaternion.LookRotation(dir);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 20 * Time.deltaTime);
                    }
                    else
                    {
                        RotateLocal();
                    }
                }
                else
                {
                    RotateLocal();
                }
            }
            else
            {
                RotateLocal();
            }
        }
    }

    public void RotateLocal()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, startRotation, 20 * Time.deltaTime);
    }
}
