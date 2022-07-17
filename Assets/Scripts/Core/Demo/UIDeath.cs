using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.TDS.UI
{
    public class UIDeath : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [TextArea]
        [SerializeField] private List<string> phrases;

        [SerializeField] private Scenes scene;
        
        private void OnEnable()
        {
            text.text = phrases[Random.Range(0, phrases.Count)];
        }

        public void Restart()
        {
            World.LoadLevel(scene);
        }
    }
}
