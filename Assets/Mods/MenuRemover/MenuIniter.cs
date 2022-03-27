using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuRemover
{
    public class MenuIniter : ModInit
    {
        public void Start()
        {
            
            Destroy(GameObject.Find("Env").gameObject);
        }
    }
}
