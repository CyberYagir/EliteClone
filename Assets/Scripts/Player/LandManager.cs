using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LandManager : MonoBehaviour
{
    public bool isLanded;
    private Vector3 landPoint;
    private Quaternion landRot;


    public void SetLand(bool land, Vector3 point = default, Quaternion rot = default)
    {
        landPoint = point;
        landRot = rot;
        isLanded = land;
        var rb = Player.inst.GetComponent<Rigidbody>();
        rb.isKinematic = isLanded;

        if (isLanded)
        {
            Player.inst.StopAxis();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            transform.DOMove(transform.position + (transform.up * 10), 1f);
        }
    }

    public void SetLand(LandLocation landLocation)
    {
        if (landLocation != null)
        {
            SetLand(true, landLocation.pos, landLocation.rot);
        }
    }

    public LandLocation GetLand()
    {
        if (isLanded)
        {
            return new LandLocation() {pos = landPoint, rot = landRot};
        }
        else
        {
            return null;
        }
    }
    
    private void Update()
    {
        Player.inst.control.enabled = !isLanded;
        if (World.Scene == Scenes.Location)
        {
            if (!isLanded)
            {
                CheckLand();
            }
            else
            {
                if (InputM.GetAxisDown(KAction.JumpIn))
                {
                    SetLand(false);
                }

                LandAnimation();
            }
        }
    }

    public void LandAnimation()
    {
        transform.position = Vector3.Lerp(transform.position, landPoint, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, landRot, Time.deltaTime);
    }
    public void CheckLand()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 10))
        {
            var land = hit.transform.GetComponent<LandPoint>();
            if (land)
            {
                WarningManager.AddWarning("Press 'Jump' to land", WarningTypes.GoLocation);
                if (InputM.GetAxisDown(KAction.JumpIn))
                {
                    SetLand(true, land.point.position, land.point.rotation);
                }
            }
        }
    }
}
