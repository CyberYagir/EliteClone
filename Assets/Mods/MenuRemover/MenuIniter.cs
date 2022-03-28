using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuRemover
{
    public class MenuIniter : ModInit
    {
        public override void Start()
        {
            Destroy(GameObject.Find("Env").gameObject);
        }
    }
}
