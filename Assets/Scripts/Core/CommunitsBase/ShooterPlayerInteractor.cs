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
        private List<ShooterInteractor> interactors = new List<ShooterInteractor>(5);

        public static bool interacted;

        public Event OnAddInter = new Event(); 
        public Event OnRemInter = new Event(); 
        public Event OnInteract = new Event();

        public void RemoveAll()
        {
            foreach (var interactor in interactors)
            {
                interactor.UnTrigger();
            }
            interactors.Clear();
            OnRemInter.Run();
        }
        
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
                interactor.UnTrigger();
                OnRemInter.Run();
            }
        }

        public bool IsHaveInteractors()
        {
            return interactors.Count != 0 && !interacted;
        }

        private void Update()
        {
            if (interacted == false && InputService.GetAxisDown(KAction.Interact))
            {
                if (interactors.Count != 0)
                {
                    interacted = true;
                    interactors[0].TriggerAction();
                    OnInteract.Run();
                }
            }
        }
    }
}