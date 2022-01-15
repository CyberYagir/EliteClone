using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Transform point;

    [SerializeField] private int orbitID;
    [SerializeField] private Vector3 orbitRotation;
    [SerializeField] private float speed;
    [SerializeField] private float width;
    private LineRenderer lineRenderer;
    private Camera camera;
    private bool drawed;
    private void Start()
    {
        DrawCircle(Vector3.Distance(transform.position, point.position));


        lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        camera = Camera.main;
        Rotate();
    }

    public void Rotate()
    {
        var rnd = new System.Random(orbitID + DateTime.Today.Day);
        transform.RotateAround(point.position, orbitRotation, rnd.Next(0, 360));
        transform.RotateAround(point.position, Vector3.up, rnd.Next(0, 360));
    }

    public void InitOrbit(Transform target, float orbitSpeed, int orbit, Vector3 orbitRot = default)
    {
        point = target;
        orbitID = orbit;
        speed = orbitSpeed;
        orbitRotation = orbitRot;
    }
    
    void Update()
    {
        transform.RotateAround(point.position, Vector3.up, speed * Time.deltaTime);
        
    }
    public void LateUpdate()
    {
        UpdateLine();
    }
    public void UpdateLine()
    {
        var curve = new AnimationCurve();

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            var dist = Vector3.Distance(transform.TransformPoint(lineRenderer.GetPosition(i)), camera.transform.position);
            var time = i / (float)lineRenderer.positionCount;
            var wdh = dist * width;


            if (dist < 200)
            {
                curve.AddKey(time, wdh / 2f);
            }
            else
            {
                curve.AddKey(time, wdh);
            }
        }
        lineRenderer.widthCurve = curve;
    }

    public void DrawCircle(float radius)
    {
        lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        var segments = 90;
        var line = lineRenderer;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = transform.InverseTransformPoint(point.position + new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius));
        }
        drawed = true;

        line.SetPositions(points);
    }
}
