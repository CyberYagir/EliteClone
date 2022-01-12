using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public static WindowManager Instance;

    [SerializeField] private List<CustomAnimate> customAnimate;
    private void Awake()
    {
        Instance = this;
    }

    public void OpenWindow(CustomAnimate animate)
    {
        foreach (var item in customAnimate)
        {
            if (item == animate)
            {
                item.reverse = !item.reverse;
            }
            else
            {
                item.reverse = true;
            }
        }
    }
    public void CloseWindow(CustomAnimate animate)
    {
        animate.reverse = true;
    }
}
