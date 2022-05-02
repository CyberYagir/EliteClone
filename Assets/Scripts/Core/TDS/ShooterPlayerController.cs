using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class ShooterPlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private Transform forward;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform IKPoint; 
    void FixedUpdate()
    {
        var oldY = rigidbody.velocity.y;
        var dir = new Vector3(InputM.GetAxis(KAction.Horizontal) * speed * Time.fixedDeltaTime, 0, InputM.GetAxis(KAction.Vertical) * speed * Time.fixedDeltaTime);
        var forwardDir = forward.TransformDirection(dir);
        rigidbody.velocity = forwardDir;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, oldY - 1, rigidbody.velocity.z);
        
        
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform != transform)
            {
                var targetRotation = Quaternion.LookRotation(new Vector3(hit.point.x, 0, hit.point.z) - new Vector3(transform.position.x, 0, transform.position.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.fixedDeltaTime);
                IKPoint.transform.position = hit.point + Vector3.up;
            }
        }
    }
}
