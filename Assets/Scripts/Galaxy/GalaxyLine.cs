using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyLine : MonoBehaviour
{
    Vector3 point, middle, endPoint;
    public GalaxyPoint systemPoint;
    public LineRenderer lineRenderer;
    public void Init(Vector3 p, Vector3 m, Vector3 e, GalaxyPoint p1)
    {
        if (point != p)
        {
            point = p;
            middle = m;
            endPoint = e;
            for (int i = 0; i < lineRenderer.positionCount; i++)
            {
                lineRenderer.SetPosition(i, point);
            }
        }

        this.systemPoint = p1;
        gameObject.SetActive(true);
        lineRenderer.SetPosition(1, Vector3.Lerp(lineRenderer.GetPosition(1), middle, 1));
        lineRenderer.SetPosition(2, Vector3.Lerp(lineRenderer.GetPosition(2), endPoint, 1));
    }

    private void Update()
    {
        if (systemPoint.particles.isStopped)
        {
            gameObject.SetActive(false);
        }
    }
}
