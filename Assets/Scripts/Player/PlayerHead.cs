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
            LookHead();
        }
        else
        {
            HeadBack();
        }
    }

    public void HeadBack()
    {
        var localRotation = transform.localRotation;
        localRotation = Quaternion.Lerp(localRotation, Quaternion.identity, 10 * Time.deltaTime);
        transform.localRotation = localRotation;
        camera.localRotation = Quaternion.Lerp(localRotation, Quaternion.identity, 10 * Time.deltaTime);
    }
    public void LookHead()
    {
        var localRot = transform.localRotation;
        localRot *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * sence * Time.deltaTime, 0);
        localRot = new Quaternion(localRot.x, Mathf.Clamp(localRot.y, -0.5f, 0.5f), localRot.z, localRot.w);
        transform.localRotation = localRot;

        var camRot = camera.localRotation;
        camRot *= Quaternion.Euler(-Input.GetAxis("Mouse Y") * sence * Time.deltaTime, 0, 0);
        camRot = new Quaternion(Mathf.Clamp(camRot.x, -0.5f, 0.5f), camRot.y, camRot.z, camRot.w);
        camera.localRotation = camRot;
    }
}
