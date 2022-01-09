using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class InitOptionsControlsDrawer : MonoBehaviour
{
    [SerializeField] private GameObject item, holder;

    public Axis chageAxis;
    public bool plus;
    public bool isChange;
    
    
    private void Start()
    {
        DrawControls();
    }

    public void DrawControls()
    {
        UITweaks.ClearHolder(holder.transform);
        var input = FindObjectOfType<InputM>();
        foreach (var axis in input.GetAxesList())
        {
            var it = Instantiate(item, holder.transform).GetComponent<InitAxieItem>();
            it.Init(axis);
            it.gameObject.SetActive(true);
        }
    }

    public void ChangeAxis(Axis axis, bool isplus)
    {
        if (!isChange)
        {
            plus = isplus;
            chageAxis = axis;
            isChange = true;
        }
    }
    
    private void OnGUI()
    {
        if (isChange)
        {
            var key = UnityEngine.Event.current;
            var finalKey = KeyCode.None;
            if (key.isKey)
            {
                finalKey = key.keyCode;
            }
            else
            {
                if (key.isMouse)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var k = (KeyCode) Enum.Parse(typeof(KeyCode), "Mouse" + i);
                        if (Input.GetKey(k))
                        {
                            finalKey = k;
                            break;
                        }
                    }
                }
            }

            if (finalKey != KeyCode.None)
            {
                if (plus)
                {
                    chageAxis.plus = finalKey;
                }
                else
                {
                    chageAxis.minus = finalKey;
                }

                DrawControls();
                isChange = false;
            }
        }
    }
}
