using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaragePlayerAnimator : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Animator animator;

    private void Update()
    {
        var target = 0;
        if (rigidbody.velocity.magnitude > 1f)
        {
            target = 1;
        }

        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), target, Time.deltaTime * 5f));
    }
}
