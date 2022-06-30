using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.Dialogs
{
    public class SaveTrouple<T,K>
    {
        public T name;
        public K data;

        public SaveTrouple(T key, K data)
        {
            name = key;
            this.data = data;
        }
    }
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
            public enum ClassName
            {
                NodeReplicaData,
                NodeAutoReplicaData,
                NodeMultiReplicaData,
                NodeTriggerData,
                NodeEndData
            }
            public ClassName classname;
            
            
            public Actions action;
            
            public string nextGUID;
            
            
            
            public string triggerGUID;
            
            public class TextReplica
            {
                public string replica;
                public string nextGUID;
            }
            public List<TextReplica> nexts = new List<TextReplica>();
        }
        

        public List<NodeData> nodeData = new List<NodeData>();
        public List<NodeLink> nodesLinks = new List<NodeLink>();
        [TextArea(10, 57)]
        public string replicasJson;


        public List<NodeReplicaData> GetConvertedReplicas()
        {
            var objects = JsonConvert.DeserializeObject<List<NodeReplicaData>>(replicasJson);
           return objects;
        }
    }
}
