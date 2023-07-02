using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class SingleInitialization : MonoBehaviour
    {
        [SerializeField] private List<StartupObject> startupObjects;
        private void Awake()
        {
            foreach (var obj in startupObjects)
            {
                obj.Init(PlayerDataManager.Instance);
            }
        }


        private void Update()
        {
            foreach (var obj in startupObjects)
            {
                if (obj.enabled)
                {
                    obj.Loop();
                }
            }
        }
    }
}
