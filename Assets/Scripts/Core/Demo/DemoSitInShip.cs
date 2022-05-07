using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

namespace Core.Demo
{
    public class DemoSitInShip : MonoBehaviour
    {
        [SerializeField] private GameObject button;
            
        private void Update()
        {
            button.transform.LookAt(ShooterPlayer.Instance.controller.GetCamera().transform.position);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<ShooterPlayer>())
            {
                button.gameObject.SetActive(true);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<ShooterPlayer>())
            {
                button.gameObject.SetActive(false);
            }
        }
    }
}
