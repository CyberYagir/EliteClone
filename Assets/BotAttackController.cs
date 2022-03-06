using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BotAttackController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float speedUp;
    [SerializeField] private AttackType attackType;
    [SerializeField] private Transform mesh;

    [SerializeField] private Transform targetShip;
    
    public enum AttackType
    {
        MoveTo, MoveBack
    }
    private Vector3 target;
    private void Start()
    {
        SetPlayerPoint();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);// = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void Update()
    {
        Vector3 dir = target - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 1.5f);


        var targetAngle = Mathf.PerlinNoise((transform.position.x + transform.position.z) * 0.01f, (transform.position.y + transform.position.z) * 0.01f);
        mesh.localRotation = Quaternion.Lerp(mesh.localRotation, Quaternion.Euler(0, 0, targetAngle * 360), 5 * Time.deltaTime);
        
        
        if (attackType == AttackType.MoveTo)
        {
            if (Vector3.Distance(transform.position, target) < 5)
            {
                ChangePoint();
            }
            else
            {
                speed = Mathf.Lerp(speed, maxSpeed, speedUp * Time.deltaTime);
            }
        }
        else
        {
            speed = Mathf.Lerp(speed, maxSpeed, speedUp * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 5)
            {
                attackType = AttackType.MoveTo;
                SetPlayerPoint();
            }
        }
    }


    public void ChangePoint()
    {
        attackType = AttackType.MoveBack;
        target = transform.position + (Random.insideUnitSphere * Random.Range(300, 600));
        if (target.magnitude > 1000)
        {
            target = Vector3.zero;
        }
    }

    public void SetPlayerPoint()
    {
        target = targetShip.position + Player.inst.transform.up * 5 + -Player.inst.transform.forward *  Random.Range(100, 600);
    }

    private void OnCollisionEnter(Collision other)
    {
        ChangePoint();
        speed /= 2f;
    }
}
