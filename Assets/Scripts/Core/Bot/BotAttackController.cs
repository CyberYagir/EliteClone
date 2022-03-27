using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
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


    private List<Weapon> weapons = new List<Weapon>();
    private List<IEnumerator> weaponHolds = new List<IEnumerator>();
    public enum AttackType
    {
        MoveTo, MoveBack
    }
    private Vector3 target;
    private void Start()
    {
        SetPlayerPoint();
        var weap = GetComponentsInChildren<ShipMeshSlot>(false).ToList();
        var ship = GetComponent<BotBuilder>().GetShip();
        for (int i = 0; i < weap.Count; i++)
        {
            weap[i].SetMesh(ship.slots[i]);
            var weapon = weap[i].GetComponent<Weapon>();
            weapon.SetCustomMask(LayerMask.GetMask("ShipMesh"));
            weapon.SetCustomCamera(weapon.transform);
            weapons.Add(weapon);
        }
        weaponHolds = new List<IEnumerator>(new IEnumerator[weap.Count]);

        StartCoroutine(BlockCircleFly());
        GetComponent<BotVisual>().ActiveLights();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.fixedDeltaTime);// = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        targetShip = target;
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

        if (targetShip && Vector3.Angle(transform.forward, targetShip.position - transform.position) < 50)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].transform.LookAt(targetShip);
                weapons[i].GetComponent<Weapon>().OnHoldDown(-1);
                if (weaponHolds[i] == null)
                {
                    weaponHolds[i] = Shoot(i);
                    StartCoroutine(weaponHolds[i]);
                }
            }
        }
    }

    IEnumerator Shoot(int id)
    {
        float time = 0;
        weapons[id].SetOffcet(Random.insideUnitSphere / 10);
        while (time < 1)
        {
            weapons[id].CheckIsCurrentWeapon(-1);
            time += Time.deltaTime;
            yield return null;
        }
        weapons[id].OnHold(-1);
        weaponHolds[id] = null;
    }


    IEnumerator BlockCircleFly()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(30, 60));
            ChangePoint();
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
        if (Player.inst != null && targetShip != null)
        {
            target = targetShip.position + Player.inst.transform.up * 5 + -targetShip.forward * Random.Range(100, 600);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        ChangePoint();
        speed /= 2f;
    }
}
