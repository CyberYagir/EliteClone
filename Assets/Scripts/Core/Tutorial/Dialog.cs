using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Dialogs
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Game/Dialogs/Dialog", order = 1)]
    public class Dialog : ScriptableObject
    {
        [System.Serializable]
        public class DialogPart
        {
            public enum Character
            {
                System, Player, Teammate, Communist
            }

            public Character character;
            [TextArea(5, 60)]
            public string text;
        }

        public List<DialogPart> replicas;
    }
}
