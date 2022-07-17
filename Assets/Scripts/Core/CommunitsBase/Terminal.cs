using System.Collections;
using System.Collections.Generic;
using Core.CommunistsBase.Intacts;
using Core.TDS;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class Terminal : MonoBehaviour
    {
        [SerializeField] private Animation animation;
        public void Init()
        {
            ShooterPlayer.Instance.Disable(transform, false, true);
            ShooterPlayer.Instance.controller.CursorState(true);
        }
        
        public void Disable()
        {
            ShooterPlayer.Instance.Disable(null, true, false);
            ShooterPlayer.Instance.controller.CursorState(false);
            animation.Play("CharInfoScreenClose");
            ShooterPlayerInteractor.interacted = false;
            GetComponent<ShooterInteractor>().enabled = false;
        }
    }
}
