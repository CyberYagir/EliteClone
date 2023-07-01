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
        [System.Serializable]
        public class NodeData
        {
            public string text;
            public string nodeGUID;
            public Vector2 pos;
            public int character;
            public int nodeType;
            public int nodeAction;

            public List<PortData> ports;
            public bool entry;
            [System.Serializable]
            public class PortData
            {
                public string portText;
                public string GUID;
                public int order;
                [System.Serializable]
                public class ConnectData
                {
                    public string targetNodeGUID;
                    public string targetPortGUID;
                }

                public List<ConnectData> connections = new List<ConnectData>();
            }
        }
        
        
        [Serializable]
        public class NodeReplicaData
        {
            public string GUID;
            public string text;
            public NodeType type;
            public Characters character;
            
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
        //public List<NodeLink> nodesLinks = new List<NodeLink>();
        [TextArea(10, 57)]
        public string replicasJson;


        public List<NodeReplicaData> GetConvertedReplicas()
        {
            var objects = JsonConvert.DeserializeObject<List<NodeReplicaData>>(replicasJson);
           return objects;
        }
    }
}
