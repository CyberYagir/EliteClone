using System;
using System.Collections.Generic;
using System.Linq;
using Core.Dialogs;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using static Core.Dialogs.ExtendedDialog;
using static Core.Dialogs.ExtendedDialog.NodeReplicaData;

public class GraphSaveUtility
{
    private ExtendedDialogGraphView target;
    public static GraphSaveUtility GetInstance(ExtendedDialogGraphView view)
    {
        return new GraphSaveUtility()
        {
            target = view
        };
    }

    private List<Edge> edges => target.edges.ToList();
    private List<ExtendedNode> nodes => target.nodes.ToList().Cast<ExtendedNode>().ToList();

    public void SaveGraph(ExtendedDialog file)
    {
        if (!edges.Any()) return;

        var dialogContainer = ScriptableObject.CreateInstance<ExtendedDialog>();

        var connectedPorts = edges.Where(x => x.input.node != null).ToArray();

        for (int i = 0; i < connectedPorts.Length; i++)
        {
            var input = connectedPorts[i].input.node as ExtendedNode;
            var output = connectedPorts[i].output.node as ExtendedNode;
            
            dialogContainer.nodesLinks.Add(new NodeLink()
            {
                baseNodeGUID = output.GUID,
                portName = connectedPorts[i].output.portName,
                targetNodeGUID = input.GUID
            });
        }

        foreach (var dialogue in nodes.Where(node=>node.NodeType != NodeType.Entry))
        {
            dialogContainer.nodeData.Add(new NodeData()
            {
                nodeGUID = dialogue.GUID,
                text = dialogue.text,
                pos = dialogue.GetPosition().position, 
                character = (int)dialogue.character,
                nodeType = (int)dialogue.NodeType,
                nodeAction = (int)dialogue.actions
            });
        }

        try
        {

            dialogContainer.replicasJson = ConvertToChain();
        }
        catch (Exception e)
        {
            
            Debug.LogError("Convert To Dialog Error\n" + e.Message);
        }
        
        var path = "";
        if (file != null)
        {
            path = AssetDatabase.GetAssetPath(file);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.Refresh();
        }

        AssetDatabase.CreateAsset(dialogContainer, path);
        AssetDatabase.SaveAssets();
        
        ExtendedDialogsGraph.GetGraph().fileName = path;
        ExtendedDialogsGraph.GetGraph().file = AssetDatabase.LoadAssetAtPath<ExtendedDialog>(path);


    }


    public string ConvertToChain()
    {
        var chain = new List<NodeReplicaData>();
        var list = nodes.Where(node => node.NodeType != NodeType.Entry);
        foreach (var dialogue in list)
        {
            if (dialogue.NodeType == NodeType.Dialog)
            {
                if (dialogue.outputContainer.childCount <= 2)
                {
                    chain.Add(new NodeReplicaData()
                    {
                        GUID = dialogue.GUID,
                        text = dialogue.text,
                        type = dialogue.NodeType,
                        classname = ClassName.NodeAutoReplicaData
                    });
                }
                else
                {
                    chain.Add(new NodeReplicaData()
                    {
                        GUID = dialogue.GUID,
                        text = dialogue.text,
                        type = dialogue.NodeType,
                        classname = ClassName.NodeMultiReplicaData
                    });
                }
            }else if (dialogue.NodeType == NodeType.Action)
            {
                chain.Add(new NodeReplicaData()
                {
                    GUID = dialogue.GUID,
                    text = dialogue.text,
                    type = dialogue.NodeType,
                    action = dialogue.actions,
                    classname = ClassName.NodeTriggerData
                });
            }else if (dialogue.NodeType == NodeType.End)
            {
                chain.Add(new NodeReplicaData()
                {
                    GUID = dialogue.GUID,
                    text = dialogue.text,
                    type = dialogue.NodeType,
                    classname = ClassName.NodeEndData
                });
            }
        }
        
        foreach (var dialogue in list)
        {
            var chainPart = chain.Find(x => x.GUID == dialogue.GUID);
            if (chainPart.type == NodeType.Dialog)
            {
                if (chainPart.classname == ClassName.NodeReplicaData)
                {
                    var autoReplica = chainPart;
                    var lastPort = (dialogue.outputContainer.ElementAt(dialogue.outputContainer.childCount - 1) as Port);
                    if (lastPort.connected)
                    {
                        var connectedGUID = (lastPort.connections.ToList()[0].input.node as ExtendedNode).GUID;
                        autoReplica.nextGUID = connectedGUID;
                    }
                }
                else
                {
                    var multiReplica = chainPart;
                    multiReplica.nexts = new List<NodeReplicaData.TextReplica>();
                    for (int i = 0; i < dialogue.outputContainer.childCount; i++)
                    {
                        var port = dialogue.outputContainer.ElementAt(i) as Port;
                        if (port.portName != "Auto")
                        {
                            if (port.connected)
                            {
                                var connectedGUID = (port.connections.ToList()[0].input.node as ExtendedNode).GUID;
                                multiReplica.nexts.Add(new NodeReplicaData.TextReplica()
                                {
                                    nextGUID = connectedGUID,
                                    replica = port.portName
                                });
                            }
                        }
                    }
                }
            }else if (chainPart.type == NodeType.End)
            {
                var endNode = chainPart;
                var lastPort = dialogue.outputContainer.ElementAt(0) as Port;
                if (lastPort.connected)
                {
                    var connectedGUID = (lastPort.connections.ToList()[0].input.node as ExtendedNode)?.GUID;
                    var triggerNode = chain.Find(x => x.GUID == connectedGUID);
                    if (triggerNode.type == NodeType.Action)
                    {
                        endNode.triggerGUID = connectedGUID;
                    }
                }
            }
        }


        return JsonConvert.SerializeObject(chain, Formatting.Indented);
    }
    
    

    
    
    
    public void LoadGraph(ExtendedDialog file)
    {
        if (file != null)
        {
            ClearGraph(file);
            CreateNodes(file);
            ConnectNodes(file);
        }
    }

    private void ConnectNodes(ExtendedDialog file)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            var connections = file.nodesLinks.Where(x => x.baseNodeGUID == nodes[i].GUID).ToList();
            for (int j = 0; j < connections.Count; j++)
            {
                var targetNodeGUID = connections[j].targetNodeGUID;
                var targetNode = nodes.First(x => x.GUID == targetNodeGUID);
                LinkNodes(nodes[i].outputContainer[j].Q<Port>(), (Port) targetNode.inputContainer[0]);
                
                targetNode.SetPosition(new Rect(file.nodeData.First(x=>x.nodeGUID == targetNodeGUID).pos, ExtendedDialogGraphView.nodeSize));
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tmpEdge = new Edge()
        {
            input = input,
            output = output
        };
        
        tmpEdge?.input.Connect(tmpEdge);
        tmpEdge?.output.Connect(tmpEdge);
        
        target.Add(tmpEdge);
    }

    private void CreateNodes(ExtendedDialog file)
    {
        foreach (var node in file.nodeData)
        {
            NodeType type = (NodeType) node.nodeType;
            
            var tmpNode = target.CreateNode(type);
            
            tmpNode.GUID = node.nodeGUID;
            tmpNode.text = node.text;
            //tmpNode.title = node.text;
            
            if (type == NodeType.Action)
            {
                tmpNode.actions = (Actions) node.nodeAction;
                tmpNode.actionUIEl.SetValueWithoutNotify(tmpNode.actions);
            }

            tmpNode.character = (Characters)node.character;
            tmpNode.characterUIEl.value = (tmpNode.character);
            tmpNode.UpdateText(tmpNode.text);
            var ports = file.nodesLinks.Where(x => x.baseNodeGUID == node.nodeGUID).ToList();

            ports.ForEach(x=>
            {
                if (x.portName != "Auto" && x.portName != "Action")
                {
                    target.AddChoicePort(tmpNode, x.portName);
                }
            });
            
            tmpNode.RefreshPorts();
            tmpNode.RefreshExpandedState();
        }
    }

    private void ClearGraph(ExtendedDialog file)
    {
        nodes.Find(x => x.NodeType == NodeType.Entry).GUID = file.nodesLinks[0].baseNodeGUID;

        foreach (var node in nodes)
        {
            if (node.NodeType == NodeType.Entry) continue;
            edges.Where(x => x.input.node == node).ToList().ForEach(edge =>
            {
                target.RemoveElement(edge);
            });
            
            target.RemoveElement(node);
        }
    }
}
