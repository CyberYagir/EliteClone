using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Dialogs
{
    public enum NodeType
    {
        Dialog, End, Action, Entry
    }
    public enum Actions
    {
        None, Positive, Negative
    }
    
    public enum Characters
    {
        Main, Second
    }
    [CreateAssetMenu(fileName = "Extended Dialog", menuName = "Game/Dialogs/Variants Dialog", order = 1)]
    public class ExtendedDialog : ScriptableObject
    {
        [Serializable]
        public class NodeData
        {
            public string nodeGUID;
            public string text;
            public Vector2 pos;
            public int character;
            public int nodeType;
            public int nodeAction;
        }

        [Serializable]
        public class NodeLink
        {
            public string baseNodeGUID;
            public string portName;
            public string targetNodeGUID;
        }


        [Serializable]
        public class NodeReplicaData
        {
            public string GUID;
            public string text;
            public NodeType type;
        }

        [Serializable]
        public class NodeAutoReplicaData : NodeReplicaData
        {
            public string nextGUID;
        }
        [Serializable]
        public class NodeMultiReplicaData : NodeReplicaData
        {
            public class TextReplica
            {
                public string replica;
                public string nextGUID;
            }
            public List<TextReplica> nexts = new List<TextReplica>();
        }
        [Serializable]
        public class NodeTriggerData : NodeReplicaData
        {
            public Actions action;
        }
        [Serializable]
        public class NodeEndData: NodeReplicaData
        {
            public string triggerGUID;
        }

        public List<NodeData> nodeData = new List<NodeData>();
        public List<NodeLink> nodesLinks = new List<NodeLink>();
        [TextArea(10, 57)]
        public string replicasJson;


        public List<NodeReplicaData> GetConvertedReplicas()
        {
            var objects = JsonConvert.DeserializeObject<List<object>>(replicasJson);
            List<NodeReplicaData> replicaDatas = objects.Cast<NodeReplicaData>().ToList();
            return replicaDatas;
        }
    }
}
