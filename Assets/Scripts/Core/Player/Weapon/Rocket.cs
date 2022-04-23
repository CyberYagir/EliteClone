using System;
using System.Collections;
using System.Collections.Generic;
using Core.Bot;
using Core.PlayerScripts;
using UnityEngine;

namespace Core.PlayerScripts.Weapon
{
    public class Rocket : MonoBehaviour
    {
        private Transform target;
        private float time;
        
        [SerializeField] private GameObject explode;
        [SerializeField] private float speed, rotSpeed;
        [SerializeField] private float maxTime = 30;
        private float maxSpeed;
        private float damage;
        private bool isVisible;
        private bool player;
        public void Init(Transform damagable, float dmg, bool isPlayer)
        {
            maxSpeed = speed;
            target = damagable;
            damage = dmg;
            player = isPlayer;
            if (target != null)
            {
                var dir = target.transform.position - transform.position;
                isVisible = Vector3.Dot(dir, transform.forward) > 30 && Vector3.Angle(dir, transform.forward) < 50;

                if (target.GetComponent<IDamagable>() == null && target.GetComponentInParent<IDamagable>() == null)
                {
                    isVisible = false;
                }
            }
        }

        private void Update()
        {
            time += Time.deltaTime;

            if (target != null && target.GetComponent<Rigidbody>() != null)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < speed)
                {
                    if (target.GetComponent<Rigidbody>().velocity.magnitude < speed)
                    {
                        speed -= Time.deltaTime * 20;
                    }
                    else
                    {
                        speed += Time.deltaTime * 20;
                        speed = Mathf.Clamp(speed, 0, maxSpeed);
                    }
                }
            }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (target != null && isVisible)
            {
                Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
            }

            if (time >= maxTime)
            {
                Explode();
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            var layer = other.transform.gameObject.layer;
            if (player)
            {
                if (layer == LayerMask.NameToLayer("Main"))
                {
                    if (other.GetComponentInParent<Player>() == null)
                    {
                        Damage(other.gameObject);
                    }
                    Explode();
                }
            }
            else
            {
                if (other.GetComponentInParent<BotBuilder>() == null)
                {
                    if (layer == LayerMask.NameToLayer("ShipMesh"))
                    {
                        Damage(other.gameObject);
                    }
                }
            }
        }

        public void Damage(GameObject attaked)
        {
            var idmg = attaked.GetComponent<IDamagable>();
            if (idmg != null)
            {
                idmg.TakeDamage(damage);
            }
            else
            {
                attaked.GetComponentInParent<IDamagable>()?.TakeDamage(damage);
            }
        }

        public void Explode()
        {
            Instantiate(explode.gameObject, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
