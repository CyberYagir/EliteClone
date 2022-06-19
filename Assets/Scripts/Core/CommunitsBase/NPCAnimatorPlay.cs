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
            GetComponent<TDSPointsWaker>().SetAnim(Animator.StringToHash(animName));
        }
    }
}
