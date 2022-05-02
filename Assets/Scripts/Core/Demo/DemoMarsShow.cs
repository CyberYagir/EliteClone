using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoMarsShow : MonoBehaviour
{
    [SerializeField] private Demo demo;
    public void ShowMars()
    {
        demo.ShowMars();
        demo.StartCoroutine(demo.StartTime());
    }
}
