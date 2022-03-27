using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initer : ModInit
{
    public override void Start()
    {
        base.Start();
        Destroy(GameObject.Find("Env").gameObject);
    }
}
