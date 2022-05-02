using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoRandomIdle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().Play(Random.Range(0, 4).ToString());   
    }
}
