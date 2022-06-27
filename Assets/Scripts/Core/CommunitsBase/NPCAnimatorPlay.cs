using System;
using System.Collections;
using System.Collections.Generic;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase                                                                             
{
    public class NPCAnimatorPlay : MonoBehaviour
    {
        public string animName;

        private void Start()
        {
            Play();
        }

        public void Play()
        {
            GetComponent<TDSPointsWaker>().SetAnim(Animator.StringToHash(animName));
        }
    }
}
