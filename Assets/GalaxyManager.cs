using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyManager : MonoBehaviour
{
    public static GalaxyPoint selectedPoint;

    private void Start()
    {
        Application.targetFrameRate = 120;
    }
}
