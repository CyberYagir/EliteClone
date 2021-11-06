using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyLine : MonoBehaviour
{
    Vector3 point, middle, endPoint;
    public LineRenderer lineRenderer;
    public void Init(Vector3 p, Vector3 m, Vector3 e)
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
        lineRenderer.SetPosition(1, Vector3.Lerp(lineRenderer.GetPosition(1), middle, 1));
        lineRenderer.SetPosition(2, Vector3.Lerp(lineRenderer.GetPosition(2), endPoint, 1));
    }
}
