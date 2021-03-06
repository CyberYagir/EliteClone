using System;
using UnityEngine;

namespace Core.Galaxy
{
    public class GalaxyUIManager : MonoBehaviour
    {
        [SerializeField] private Transform selectGalaxyCursor;
        [SerializeField] private Camera camera;
        private void Update()
        {
            SelectSystemIcon();
        }

        public void SelectSystemIcon()
        {
            selectGalaxyCursor.gameObject.SetActive(GalaxyManager.selectedPoint != null);
            if (GalaxyManager.selectedPoint)
            {
                if (Vector3.Angle(camera.transform.forward, GalaxyManager.selectedPoint.transform.position - camera.transform.position) < 90)
                {
                    var oldPos = selectGalaxyCursor.transform.position;
                    selectGalaxyCursor.transform.position = camera.WorldToScreenPoint(GalaxyManager.selectedPoint.transform.position, Camera.MonoOrStereoscopicEye.Mono);
                    selectGalaxyCursor.transform.localPosition = new Vector3(selectGalaxyCursor.transform.localPosition.x, selectGalaxyCursor.transform.localPosition.y, 0);
                    var newPos = selectGalaxyCursor.transform.position;
                    selectGalaxyCursor.transform.position = oldPos;
                    selectGalaxyCursor.transform.position = Vector3.Lerp(oldPos, newPos, !Input.GetKey(KeyCode.Mouse1) ? 10f * Time.deltaTime : 100000 * Time.deltaTime);
                }
                else
                {
                    selectGalaxyCursor.gameObject.SetActive(false);
                }
            }
        }
    }
}
