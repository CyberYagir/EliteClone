using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Demo
{
    public class DemoSitInShip : MonoBehaviour
    {
        [SerializeField] private GameObject button;
        
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image image;

        [SerializeField] private List<GameObject> toDisable;
        [SerializeField] private List<GameObject> toEnable;

        private bool active;
        private void Update()
        {
            if (button.active)
            {
                button.transform.LookAt(ShooterPlayer.Instance.controller.GetCamera().transform.position);
                if (Input.GetKeyDown(KeyCode.F) && !active)
                {
                    active = true;
                    var demo = transform.root.gameObject.GetComponent<Demo>();
                    foreach (var anim in transform.root.GetComponentsInChildren<Animator>())
                    {
                        anim.enabled = false;
                    }
                    demo.StartCoroutine(Animation());
                }
            }
        }

        IEnumerator Animation(){
            canvas.gameObject.SetActive(true);
            image.DOFade(1, 0.5f);
            yield return new WaitForSeconds(0.5f);
            transform.root.gameObject.GetComponent<Animator>();
            for (int i = 0; i < toEnable.Count; i++)
            {
                toEnable[i].gameObject.SetActive(true);
            }

            for (int i = 0; i < toDisable.Count; i++)
            {
                toDisable[i].gameObject.SetActive(false);
            }
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
