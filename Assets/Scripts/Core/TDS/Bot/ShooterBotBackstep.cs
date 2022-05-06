using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.TDS.Bot
{
    [RequireComponent(typeof(ShooterBotLookBack))]
    public class ShooterBotBackstep : MonoBehaviour
    {
        public Transform button;

        public void InBack()
        {
            button.transform.position= Vector3.Lerp(button.transform.position, ShooterPlayer.Instance.transform.position, 0.5f) + Vector3.up;
            button.transform.LookAt(ShooterPlayer.Instance.controller.GetCamera().transform.position);
            button.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                GetComponent<Shooter>().Death();
            }
        }

        public void OutBack()
        {
            button.gameObject.SetActive(false);
        }
    }
}
