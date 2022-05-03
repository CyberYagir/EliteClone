using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Core.TDS
{
    [RequireComponent(typeof(ShooterBotLookBack))]
    public class ShooterBotBackstep : MonoBehaviour
    {
        private ShooterBotLookBack backLook;

        public Transform button;
        private void Start()
        {
            backLook = GetComponent<ShooterBotLookBack>();
        }

        public void InBack()
        {
            button.transform.position= Vector3.Lerp(button.transform.position, ShooterPlayer.Instance.transform.position, 0.5f) + Vector3.up;
            button.transform.LookAt(ShooterPlayer.Instance.followCamera.transform.position);
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
