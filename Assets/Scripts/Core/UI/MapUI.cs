using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Map;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.UI
{
    public class MapUI : MonoBehaviour
    {
        private void Start()
        {
            if (!MapGenerator.Set)
            {
                MapGenerator.Set = true;
                SceneManager.LoadScene("Map", LoadSceneMode.Additive);
            }
        }
        
    }
}
