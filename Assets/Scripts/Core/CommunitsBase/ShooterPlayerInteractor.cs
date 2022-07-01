using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.CommunistsBase.Intacts
{
    public class ShooterPlayerInteractor : MonoBehaviour
    {
        [SerializeField]
        private List<ShooterInteractor> interactors = new List<ShooterInteractor>();

        public static bool interacted;

        public Event OnAddInter = new Event(); 
        public Event OnRemInter = new Event(); 
        public Event OnInteract = new Event(); 

        public void AddInteractor(ShooterInteractor interactor)
        {
            if (!interactors.Contains(interactor))
            {
                interactors.Add(interactor);
                OnAddInter.Run();
            }
        }

        public void DestroyInteractor(ShooterInteractor interactor)
        {
            if (interactors.Remove(interactor))
            {
                OnRemInter.Run();
            }
        }

        public bool IsHaveInteractors()
        {
            return interactors.Count != 0 && !interacted;
        }

        private void Update()
        {
            if (interacted == false && InputM.GetAxisDown(KAction.Interact))
            {
                if (interactors.Count != 0)
                {
                    interactors[0].TriggerAction();
                    interacted = true;
                    OnInteract.Run();
                }
            }
        }
    }
}