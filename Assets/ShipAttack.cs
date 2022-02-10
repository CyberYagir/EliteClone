using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAttack : MonoBehaviour
{
    public Event<int> OnShoot = new Event<int>();
    void Update()
    {
        for (int i = 0; i < 9; i++)
        {
            if (Input.GetKey(i.ToString()) || (i == 1 && Input.GetKey(KeyCode.Mouse0)) || (i == 2 && Input.GetKey(KeyCode.Mouse1)))
            {
                OnShoot.Run(i);
            }
        }
    }
}
