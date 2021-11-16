using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Player.inst.GetTarget())
                transform.position = Player.inst.GetTarget().transform.position;
        }
    }
}
