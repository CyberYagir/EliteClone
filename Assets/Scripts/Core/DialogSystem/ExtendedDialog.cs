using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Dialogs
{
    [CreateAssetMenu(fileName = "Extended Dialog", menuName = "Game/Dialogs/Variants Dialog", order = 1)]
    public class ExtendedDialog : ScriptableObject
    {
        [Serializable]
        public class NodeData
        {
            public string nodeGUID;
            public string text;
            public Vector2 pos;
        }

        [Serializable]
        public class NodeLink
        {
            public string baseNodeGUID;
            public string portName;
            public string targetNodeGUID;
        }

        public List<NodeData> nodeData = new List<NodeData>();
        public List<NodeLink> nodesLinks = new List<NodeLink>();
    }
}
