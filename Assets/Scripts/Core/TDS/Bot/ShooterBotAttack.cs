using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using Pathfinding;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

namespace Core.TDS.Bot
{
    public class ShooterBotAttack : MonoBehaviour
    {
        [SerializeField] private Rig rig;
        [SerializeField] private Transform point;
        [SerializeField] private float repointTime;
        [SerializeField] private float shootcooldown;
        [SerializeField] private float minDist, maxDist;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private TDSBullets bullets;
        
        
        private float nextTime = 0;
        private float nextShootTime = 0;
        private Transform target;
        private float time = 0;
        private float shootTime = 0;
        private void Start()
        {
            time = 99999;
            agent.updateRotation = false;
            nextTime = repointTime + Random.Range(0, nextTime * 0.5f);
            nextShootTime = shootcooldown * Random.Range(1f, 2f);
        }

        public void Calculate()
        {
            if (rig.weight != 1)
            {
                rig.weight = Mathf.Clamp01(rig.weight + Time.deltaTime);
            }

            transform.DOLookAt(ShooterPlayer.Instance.transform.position, 0.2f);
            point.position = ShooterPlayer.Instance.transform.position + Vector3.up;

            time += Time.deltaTime;
            shootTime += Time.deltaTime;

            if (shootTime > nextShootTime)
            {
                bullets.GetParticles().Emit(1);
                nextShootTime = shootcooldown * Random.Range(1f, 2f);
                shootTime = 0;
            }
            
            if (time > nextTime)
            {
                var twoDDir = Random.insideUnitCircle;
                var dir = new Vector3(twoDDir.x, 0, twoDDir.y);
                var pos = ShooterPlayer.Instance.transform.position + dir * Random.Range(minDist, maxDist);

                if (target != null)
                {
                    Destroy(target.gameObject);
                }
                agent.SetDestination(pos);
                time = 0;
            }   
        }
    }
}
