using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Transform point;

    public int orbitID;
    public Vector3 orbitRotation;
    public float speed;
    public float width;
    LineRenderer lineRenderer;
    Camera camera;
    bool drawed;
    private void Start()
    {
        if (!drawed)
            DrawCircle(Vector3.Distance(transform.position, point.position));


        lineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
        camera = Camera.main;
        var rnd = new System.Random(orbitID + DateTime.Today.Day);

        transform.RotateAround(point.position, orbitRotation, rnd.Next(0, 360));
        transform.RotateAround(point.position, Vector3.up, rnd.Next(0, 360));
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
