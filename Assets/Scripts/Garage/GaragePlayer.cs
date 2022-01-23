using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaragePlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float speed;
    [SerializeField] private Transform forward;
    void FixedUpdate()
    {
        var oldY = rigidbody.velocity.y;
        var dir = new Vector3(InputM.GetAxis(KAction.Horizontal) * speed * Time.fixedDeltaTime, 0, InputM.GetAxis(KAction.Vertical) * speed * Time.fixedDeltaTime);
        var forwardDir = forward.TransformDirection(dir);
        rigidbody.velocity = forwardDir;
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, oldY - 1, rigidbody.velocity.z);
        if (forwardDir != Vector3.zero)
        {
            var targetRotation = Quaternion.LookRotation(forwardDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.fixedDeltaTime);
        }

        if(Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
        {
            transform.parent = hit.transform;
        }
    }
}
