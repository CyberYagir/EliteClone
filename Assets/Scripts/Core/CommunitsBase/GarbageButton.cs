using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase.Intacts;
using UnityEngine;

namespace Core
{
    public class GarbageButton : MonoBehaviour
    {
        [SerializeField] private bool isOn = true;
        [SerializeField] private Animator animator;
        [SerializeField] private Material onMat, offMat;
        [SerializeField] private Renderer buttonMesh;

        public void ChangeState()
        {
            isOn = !isOn;

            animator.enabled = isOn;

            var mat = buttonMesh.sharedMaterials;
            mat[1] = isOn ? onMat : offMat;
            buttonMesh.sharedMaterials = mat;

            ShooterPlayerInteractor.interacted = false;
        }
        
    }
}
