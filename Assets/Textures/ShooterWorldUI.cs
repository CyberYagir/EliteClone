using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

public class ShooterWorldUI : MonoBehaviour
{
    public Transform energy, heath;
    private float maxScaleEnergy, maxScaleHeath;

    private void Start()
    {
        ShooterPlayer.Instance.data.UpdateData += UpdateUI;
        maxScaleEnergy = energy.localScale.y;
        maxScaleHeath = heath.localScale.y;
    }

    public void UpdateUI()
    {
        energy.localScale = new Vector3(energy.localScale.x, ShooterPlayer.Instance.data.GetEnergy() * maxScaleEnergy, energy.localScale.z) ;
        heath.localScale = new Vector3(heath.localScale.x, ShooterPlayer.Instance.data.GetHealth() * maxScaleHeath, heath.localScale.z) ;
    }
}
