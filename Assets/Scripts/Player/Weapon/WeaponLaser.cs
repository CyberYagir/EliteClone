using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

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
        particles.SetActive(false);
    }

    public void SetLines(Vector3 pos)
    {
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, cacheHolder.transform.InverseTransformPoint(pos));
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
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out RaycastHit hit))
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

        if (Physics.Raycast(transform.position, hitPoint - transform.position, out RaycastHit hit1))
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
                SpawnDecal(options.attackDecal, lastPos, lastDir, lastHit);
                particles.SetActive(hitted);
            }

            if (particles.activeSelf)
            {
                particles.transform.position = hitPoint;
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
