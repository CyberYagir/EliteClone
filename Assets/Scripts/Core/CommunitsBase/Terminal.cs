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
        [SerializeField] private bool used;
        public Event OnDisable = new Event();
        public void Init()
        {
            ShooterPlayer.Instance.Disable(transform, false, true);
            ShooterPlayer.Instance.controller.CursorState(true);
            used = true;
        }

        public void Disable()
        {
            if (used)
            {
                ShooterPlayer.Instance.Disable(null, true, false);
                ShooterPlayer.Instance.controller.CursorState(false);
                animation.Play("CharInfoScreenClose");
                ShooterPlayerInteractor.interacted = false;
                GetComponent<ShooterInteractor>().enabled = false;
                OnDisable.Run();
            }
        }
    }
}
