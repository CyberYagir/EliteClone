using System;
using System.Collections;
using System.Collections.Generic;
using Core.Map;
using UnityEngine;

namespace Core.UI
{
    public class MapCameraRotator : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private MapSelect select;

        private void Update()
        {
            if (MapGenerator.mode == MapSelect.MapMode.Active && MapSelect.selected)
            {
                if (InputM.GetPressButton(KAction.MapRotate))
                {
                    transform.RotateAround(MapSelect.selected.transform.position,
                        transform.up,
                        -Input.GetAxis("Mouse X") * speed * -Time.deltaTime);

                    transform.RotateAround(MapSelect.selected.transform.position,
                        transform.right,
                        -Input.GetAxis("Mouse Y") * speed * Time.deltaTime);
                    
                    select.UpdatePoint();
                }
            }
        }
    }
}
