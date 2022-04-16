using System.Collections;
using Core.Location;
using DG.Tweening;
using UnityEngine;

namespace Core.Bot
{
    public class BotLandController : MonoBehaviour
    {
        public bool isLandProcess;

        private float time;
        private float inLandTime;
        private bool unLand;
        private void Start()
        {
            inLandTime = Random.Range(30, 300);
        }

        private LandPoint point;

        public void SetLandPoint(LandPoint p)
        {
            point = p;
        }

        private void Update()
        {
            if (!isLandProcess)
            {
                time += Time.deltaTime;
                if (time > inLandTime)
                {
                    if (!unLand)
                    {
                        Unland();
                    }
                    unLand = true;
                }
            }
        }

        IEnumerator Animation()
        {
            transform.DOMove(transform.position + (transform.up * 2), 1f);
            yield return new WaitForSeconds(1);
            transform.DOLocalRotateQuaternion(transform.localRotation * Quaternion.Euler(-Random.Range(60, 160), 0, 0), 1);
            GetComponent<BotVisual>().ActiveLights();
            while (time < 20)
            {
                yield return null;
                transform.Translate(Vector3.forward * 30 * Time.deltaTime);
            }

            transform.DOScale(Vector3.zero, 2);
            while (transform.localScale.magnitude > 0.001f)
            {
                yield return null;
                transform.Translate(Vector3.forward * 30 * Time.deltaTime);
            }

            var particles = GetComponent<BotBuilder>().PlayWarp();
            particles.transform.parent = null;
            particles.transform.localScale = Vector3.one;
        
            Destroy(particles.gameObject, 2);
        
            Destroy(gameObject);
        }
    
        public void Unland()
        {
            StartCoroutine(Animation());
        }

        private void OnDisable()
        {
            if (point)
            {
                point.isFilled = false;
            }
        }
    }
}
