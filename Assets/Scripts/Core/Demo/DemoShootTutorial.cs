using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

namespace Core
{
    public class DemoShootTutorial : MonoBehaviour
    {
        [SerializeField] private Transform key;
        // Start is called before the first frame update
        void Start()
        {
            key.gameObject.SetActive(false);
            GetComponent<ShooterInventory>().OnChange += StopTime;
        }

        private float time = 0;
        private bool wait;
        public void StopTime()
        {
            wait = true;
        }
        
        private void LateUpdate()
        {
            if (wait)
            {
                time += Time.deltaTime;
                if (time >= 0.5f)
                {
                    key.gameObject.gameObject.SetActive(true);
                }
            }
            if (key.gameObject.active)
            {
                key.transform.LookAt(ShooterPlayer.Instance.controller.GetCamera().transform);
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Time.timeScale = 1;
                    key.gameObject.SetActive(false);
                    Destroy(this);
                }
            }
        }
    }
}
