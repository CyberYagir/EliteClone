using System;
using System.Collections;
using Core.PlayerScripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Demo
{
    public class Demo : MonoBehaviour
    {
        private bool focus = true;
        
        [SerializeField] private RectTransform textField;
        [SerializeField] private GameObject firstModel, secondModel;
        [SerializeField] private Animator secondAnimator;
        [SerializeField] private GameObject cube, mars;
        [SerializeField] private ParticleSystem shootParticles;
        [SerializeField] private Camera camera;
        [SerializeField] private Image skipImage;
        private float skipTime;
        
        private float textShowTimer;
        private bool timerShowed;

        private float time;


        private float lateTime = 0;

        private void Awake()
        {
            StartCoroutine(MarsWaiter());
        }

        public void ShowText()
        {
            StartCoroutine(DemoWaiter());
        }


        private void LateUpdate()
        {
            if (Input.GetKey(KeyCode.Space))
            {
                skipTime += Time.unscaledDeltaTime;
                if (skipTime >= 5)
                {
                    DemoMoveToGalaxy.LoadGalaxyLocation();
                    skipTime = -1000;
                    return;
                }
            }
            else skipTime = 0;
            lateTime += Time.unscaledDeltaTime;
            skipImage.fillAmount = skipTime / 5f;
        }

        IEnumerator DemoWaiter()
        {

            while (!Input.GetKeyDown(KeyCode.Return) || textField.GetComponent<TMP_InputField>().text == "")
            {
                yield return null;
                textField.anchoredPosition = Vector2.Lerp(textField.anchoredPosition, new Vector2(544, 186), 5 * Time.unscaledDeltaTime);
            }

            textField.DOAnchorPos(new Vector2(544, -1000), 0.2f).SetUpdate(UpdateType.Late);
            StartCoroutine(StartTime());
            lateTime = 0;
            while (lateTime < 2f)
            {
                yield return null;
            }

            secondAnimator.Play("Death", 2);
            float weight = 0;
            while (secondAnimator.GetLayerWeight(2) < 1)
            {
                weight += Time.unscaledDeltaTime * 2;
                secondAnimator.SetLayerWeight(2, weight);
                yield return null;
            }
        }

        IEnumerator MarsWaiter()
        {
            yield return new WaitForSeconds(9.3f);
            yield return StartCoroutine(StopTimer());
            camera.GetComponent<Animator>().enabled = true;

        }

        public void ShowMars()
        {
            mars.SetActive(true);
            cube.SetActive(false);
            shootParticles.gameObject.SetActive(false);
            mars.SetActive(true);
            cube.SetActive(false);
        }

        public void OnSecond()
        {
            firstModel.SetActive(false);
            secondModel.SetActive(true);
        }

        public IEnumerator CheckFocus()
        {
            bool notHaveFocus = false;
            while (!focus)
            {
                notHaveFocus = true;
                yield return null;
            }

            if (notHaveFocus)
            {
                uscaled = 0;
                yield return null;
            }
        }
        float uscaled = 0;
        public IEnumerator StartTime()
        {
            while (Time.timeScale < 1)
            {
                uscaled = Time.unscaledDeltaTime;
                yield return CheckFocus();
                if (Time.timeScale + uscaled > 1)
                {
                    Time.timeScale = 1;
                    yield break;
                }
                Time.timeScale += uscaled;
                yield return null;
            }
        }

        public IEnumerator StopTimer()
        {
            while (Time.timeScale > 0)
            {
                uscaled = Time.unscaledDeltaTime;
                yield return CheckFocus();
                if (Time.timeScale - uscaled * 2 < 0)
                {
                    Time.timeScale = 0;
                    yield break;
                }
                Time.timeScale -= uscaled * 2;
                yield return null;
            }
        }


        private void OnApplicationFocus(bool hasFocus)
        {
            focus = hasFocus;
        }
    }
}
