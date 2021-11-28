using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    [SerializeField] float sence;
    [SerializeField] Transform camera;
    Player player;

    private void Start()
    {
        player = Player.inst;
    }

    void Update()
    {
        if (player.control.headView)
        {
            transform.localRotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sence * Time.deltaTime, 0);
            transform.localRotation = new Quaternion(transform.localRotation.x, Mathf.Clamp(transform.localRotation.y, -0.5f, 0.5f), transform.localRotation.z, transform.localRotation.w);
            camera.localRotation *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * sence * Time.deltaTime, 0, 0);
            camera.localRotation = new Quaternion(Mathf.Clamp(camera.localRotation.x, -0.5f, 0.5f), camera.localRotation.y, camera.localRotation.z, camera.localRotation.w);
        }
        else
        {
            var localRotation = transform.localRotation;
            localRotation = Quaternion.Lerp(localRotation, Quaternion.identity, 10 * Time.deltaTime);
            transform.localRotation = localRotation;
            camera.localRotation = Quaternion.Lerp(localRotation, Quaternion.identity, 10 * Time.deltaTime);
        }
    }
}
