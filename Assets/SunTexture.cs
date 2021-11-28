using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunTexture : MonoBehaviour
{
    private float scroll;
    private float scrollspeed = 0.01f;
    void Update()
    {
        scroll += Time.deltaTime * scrollspeed;
        
        GetComponent<Renderer>().material.SetTextureOffset("_BaseColorMap", new Vector2(scroll, 1));
    }
}
