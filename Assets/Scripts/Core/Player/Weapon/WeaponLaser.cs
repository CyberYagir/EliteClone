using Core.Systems;
using UnityEngine;

namespace Core.PlayerScripts.Weapon
{
    public class WeaponLaser : Weapon
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private GameObject particles;
        private RaycastHit lastHit;
        protected override void InitData()
        {
            line = cacheHolder.AddComponent<LineRenderer>();
            line.enabled = false;
            line.material = options.laserOptions.materal;
            line.widthCurve = options.laserOptions.width;
            line.renderingLayerMask = 256;
            line.useWorldSpace = false;
            particles = Instantiate(options.attackParticles);
            particles.transform.parent = cacheHolder.transform;
            var light = particles.GetComponentInChildren<Light>();
            if (light != null)
            {
                light.color = options.laserOptions.materal.color;
            }
            particles.SetActive(false);
        }

        public void SetLines(Vector3 pos)
        {
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, cacheHolder.transform.InverseTransformPoint(pos) + pointOffcet);
        }

        protected override void AttackDown()
        {
            var point = GetPoint();
            SetLines(point);
            if (particles.activeSelf)
            {
                particles.transform.position = point;
            }
        
        }

        protected override void Attack()
        {
            line.enabled = true;
            var hitPoint = GetPoint();

            var globalPos = cacheHolder.transform.TransformPoint(line.GetPosition(1));
            SetLines(Vector3.Lerp(globalPos, hitPoint, 10 * Time.deltaTime));
        }

        public Vector3 GetPoint()
        {
        
            lastHit = new RaycastHit();
            var lastPos = new Vector3();
            var lastDir = new Vector3();
            var hitPoint = camera.transform.position + camera.transform.forward * 500;
            bool hitted = false;
            var layer = LayerMask.GetMask("Default", "Main");
            if (isLayerMaskChanged)
            {
                layer = customMask;
            }
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit, options.maxDistance, layer, QueryTriggerInteraction.Ignore))
            {
            
                if (hit.transform.GetComponent<TexturingScript>() == null)
                {
                    lastHit = hit;
                    lastPos = camera.transform.position;
                    lastDir = camera.transform.forward;
                    hitPoint = hit.point;
                    hitted = true;
                }
            }

            if (Physics.Raycast(transform.position, hitPoint - transform.position, out RaycastHit hit1, options.maxDistance, layer, QueryTriggerInteraction.Ignore))
            {
                if (hit1.transform.GetComponent<TexturingScript>() == null)
                {
                    lastHit = hit;
                    lastPos = transform.position;
                    lastDir = hitPoint - transform.position;
                    hitPoint = hit1.point;
                    hitted = true;
                }
            }
        
            if (options.maxDistance >= lastHit.distance)
            {
                if (line.enabled)
                {
                    if (lastHit.collider != null)
                        SpawnDecal(options.attackDecal, lastPos, lastDir, lastHit);
                    particles.SetActive(hitted);
                }

                if (particles.activeSelf)
                {
                    particles.transform.position = hitPoint;
                    particles.transform.LookAt(Player.inst.transform);
                }
            }
            else
            {
                particles.SetActive(false);
            }

            return hitPoint;
        }

        protected override void NotAttack()
        {
            line.enabled = false;
            particles.SetActive(false);
        }
    }
}
