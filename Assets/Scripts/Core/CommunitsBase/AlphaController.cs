using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.TDS;
using Core.TDS.Bot;
using DG.Tweening;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class AlphaController : MonoBehaviour
    {
        [System.Serializable]
        public partial class Box
        {
            public Vector3 pos, size;
        }
        public List<Box> casts;
        public List<Transform> rooms;
        public List<Transform> players = new List<Transform>(10);
        [Space]
        public bool overrideActive;

        
        

        private void OnDrawGizmos()
        {
            for (int i = 0; i < casts.Count; i++)
            {
                Gizmos.DrawWireCube((transform.position + casts[i].pos), casts[i].size);
            }
        }
    }
}
