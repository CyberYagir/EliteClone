using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KAction { Horizontal, Vertical, Tabs, TabsVertical, TabsHorizontal, Select, Click, GalaxyVertical, HeadView, SetTarget, JumpIn, StartWarp, Stop}
[System.Serializable]
public class Axis
{
    public KAction action;
    public KeyCode plus, minus;
    public float value;
    public float transitionSpeed = 5;
    public int rawvalue;
    public bool down;
    public bool up;
}

public class InputM : MonoBehaviour
{
    public List<Axis> axes;
    public static Dictionary<KAction, Axis> keys = new Dictionary<KAction, Axis>();

    private void Awake()
    {
        keys = new Dictionary<KAction, Axis>();
        foreach (var ax in axes)
        {
            keys.Add(ax.action, ax);
        }
    }

    private void Update()
    {
        foreach (var axies in keys)
        {
            var ax = axies.Value;
            ax.rawvalue = 0;
            if (ax.plus != KeyCode.None)
            {
                ax.rawvalue += Input.GetKey(ax.plus) ? 1 : 0;
                ax.down = Input.GetKeyDown(ax.plus);
                ax.up = Input.GetKeyUp(ax.plus);
            }
            if (ax.minus != KeyCode.None)
            {
                if (!ax.down)
                {
                    ax.down = Input.GetKeyDown(ax.minus);
                }
                if (!ax.up)
                {
                    ax.up = Input.GetKeyUp(ax.minus);
                }
                ax.rawvalue -= Input.GetKey(ax.minus) ? 1 : 0;
            }
            ax.value = Mathf.Lerp(ax.value, ax.rawvalue, ax.transitionSpeed * Time.deltaTime);
        }
    }

    public static float GetAxis(KAction action)
    {
        return keys[action].value;
    }
    public static bool GetAxisChange(KAction action)
    {
        return keys[action].rawvalue != 0;
    }
    public static int GetAxisRaw(KAction action)
    {
        return keys[action].rawvalue;
    }
    /// <summary>
    /// Hold button
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool GetPressButton(KAction action)
    {
        return keys[action].rawvalue != 0;
    }
    /// <summary>
    /// One frame down
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public static bool GetAxisDown(KAction action)
    {
        return keys[action].down;
    }
    public static bool GetAxisUp(KAction action)
    {
        return keys[action].up;
    }
    public static Axis GetAxisData(KAction action)
    {
        return keys[action];
    }
}