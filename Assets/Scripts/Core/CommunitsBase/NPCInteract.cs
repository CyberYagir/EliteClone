using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.CommunistsBase
{
    public class NPCInteract : MonoBehaviour
    {
        public Event ActionEvent;


        public void OnAction()
        {
            ActionEvent.Invoke();
        }

        public void Clear()
        {
            ActionEvent = new Event();
        }
    }
}
