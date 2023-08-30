using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
    public class ModsButton : MonoBehaviour
    {
        void Start()
        {
            if (ModsManager.Instance.modLoader.mods.Count == 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
