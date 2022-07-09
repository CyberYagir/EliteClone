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

    
    public void SaveGraphNew(ExtendedDialog file)
    {
        var dialogContainer = ScriptableObject.CreateInstance<ExtendedDialog>();
        foreach (var dialogue in nodes.Where(node => node.NodeType != NodeType.Entry))
        {
            var nodeData = new NodeData()
            {
                nodeGUID = dialogue.GUID,
                text = dialogue.text,
                pos = dialogue.GetPosition().position,
                character = (int) dialogue.character,
                nodeType = (int) dialogue.NodeType,
                nodeAction = (int) dialogue.actions,
                ports = new List<NodeData.PortData>()
            };

            foreach (var obj in dialogue.outputContainer.Children())
            {
                if (obj is Port)
                {
                    var port = obj as Port;
                    var data = port.Q<DataElement>();
                    var portData = new NodeData.PortData()
                    {
                        portText = data.text,
                        order = data.order,
                        GUID = data.GetGUID(),
                        connections = new List<NodeData.PortData.ConnectData>()
                    };
                    if (port.connected)
                    {
                        foreach (var edge in port.connections)
                        {
                            portData.connections.Add(new NodeData.PortData.ConnectData()
                            {
                                targetNodeGUID = (edge.input.node as ExtendedNode).GUID,
                                targetPortGUID = (edge.input).Q<DataElement>().GetGUID()
                            });
                        }
                    }

                    nodeData.ports.Add(portData);
                }



                
                // if (dialogue.text == "Thanks to the Ancients for Lenin Marx and Engels!")
                // {
                //     if (dialogue.inputContainer.childCount != 0)
                //     {
                //         var inputPort = (dialogue.inputContainer.ElementAt(0) as Port);
                //         if (inputPort.connected)
                //         {
                //             foreach (var connection in inputPort.connections)
                //             {
                //                 Debug.Log((connection.input.node as ExtendedNode).NodeType);
                //                 if ((connection.input.node as ExtendedNode).NodeType == NodeType.Entry)
                //                 {
                //                     nodeData.entry = true;
                //                     break;
                //                 }
                //             }
                //         }
                //     }
                // }
            }

           
            if (nodeData.ports.Count != 0)
            {
                nodeData.ports = nodeData.ports.OrderBy(x => x.order).ToList();
            }

            
            dialogContainer.nodeData.Add(nodeData);
            
        }
        
                    
        var start = nodes.Find(x => x.NodeType == NodeType.Entry).outputContainer.ElementAt(0) as Port;
        if (start.connected)
        {
            var node = start.connections.ElementAt(0).input.node as ExtendedNode;
            if (node != null)
            {
                dialogContainer.nodeData.Find(x => x.nodeGUID == node.GUID).entry = true;
            }
        }

        var entry = dialogContainer.nodeData.Find(x => x.entry);
        if (entry != null)
        {
            dialogContainer.nodeData.RemoveAll(x => x.nodeGUID == entry.nodeGUID);
            dialogContainer.nodeData.Insert(0, entry);
        }

        try
        {

            dialogContainer.replicasJson = ConvertToChain(entry?.nodeGUID);
        }
        catch (Exception e)
        {
            
            Debug.LogError("Convert To Dialog Error\n" + e.Message);
        }

        var path = "";
        if (file != null)
        {
            path = AssetDatabase.GetAssetPath(file);
            EditorUtility.CopySerialized(dialogContainer, AssetDatabase.LoadAssetAtPath<ExtendedDialog>(path));
        }
        else
        {
            AssetDatabase.CreateAsset(dialogContainer, path);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

        ExtendedDialogsGraph.GetGraph().fileName = path;
        ExtendedDialogsGraph.GetGraph().file = AssetDatabase.LoadAssetAtPath<ExtendedDialog>(path);
    }
    

    public void LoadGraphNew(ExtendedDialog file)
    {
        if (file != null)
        {
            ClearGraph(file);
            CreateNewNodes(file);
            ConnectNewNodes(file);
        }
    }

    private void CreateNewNodes(ExtendedDialog file)
    {
        foreach (var node in file.nodeData)
        {
            NodeType type = (NodeType) node.nodeType;

            var tmpNode = target.CreateNode(type);

            tmpNode.GUID = node.nodeGUID;
            tmpNode.text = node.text;

            if (type == NodeType.Action)
            {
                tmpNode.actions = (Actions) node.nodeAction;
                tmpNode.actionUIEl.SetValueWithoutNotify(tmpNode.actions);
            }

            tmpNode.character = (Characters) node.character;
            tmpNode.characterUIEl.value = (tmpNode.character);
            tmpNode.UpdateText(tmpNode.text);

            tmpNode.SetPosition(new Rect(node.pos, ExtendedDialogGraphView.nodeSize));
            foreach (var port in node.ports.OrderBy(x=>x.order))
            {
                if (port.portText != "Auto" && port.portText != "Action")
                {
                    target.AddChoicePort(tmpNode, port.portText, port.order, port.GUID);
                }
            }


            SetPortByNameGUID(node, tmpNode, "Auto");
            SetPortByNameGUID(node, tmpNode, "Action");


            tmpNode.RefreshPorts();
            tmpNode.RefreshExpandedState();
        }
    }


    public void SetPortByNameGUID(NodeData node, ExtendedNode tmpNode, string portName)
    {
        var autoPort = node.ports.Find(x => x.portText == portName);
        if (autoPort != null){
            foreach (var port in tmpNode.outputContainer.Children())
            {
                if (port is Port)
                {
                    var ordered = port as Port;
                    if (ordered.portName == portName)
                    {
                        ordered.Q<DataElement>().SetGUID(autoPort.GUID);
                    }
                }
            }
        } 
    }

    private void ConnectNewNodes(ExtendedDialog file)
    {
        foreach (var node in file.nodeData)
        {
            var createdNode = nodes.Find(x => x.GUID == node.nodeGUID);
            if (node.entry)
            {
                LinkNodes(nodes.Find(x=>x.NodeType == NodeType.Entry).outputContainer.ElementAt(0) as Port, createdNode.inputContainer.ElementAt(0) as Port);
            }
            
            foreach (var port in node.ports)
            {
                Port findedPort = FindPort(createdNode, port.GUID, createdNode.outputContainer.Children().ToList());
                if (findedPort != null)
                {
                    foreach (var conn in port.connections)
                    {
                        var targetNode = nodes.Find(x => x.GUID == conn.targetNodeGUID);
                        var targetPort = targetNode.inputContainer.ElementAt(0) as Port;
                        if (targetPort != null)
                        {
                            LinkNodes(findedPort, targetPort);
                        }
                    }
                }
            }
        }
    }

    public Port FindPort(ExtendedNode createdNode, string portGUID, List<VisualElement> container)
    {
        foreach (var x in container)
        {
            if (x is Port)
            {
                var ordered = (x as Port);
                if (ordered.Q<DataElement>().GetGUID() == portGUID)
                {
                    return ordered;
                }
            }

        }

        return null;
    }

    public string ConvertToChain(string entryGUID)
    {
        var chain = new List<NodeReplicaData>();
        var list = nodes.Where(node => node.NodeType != NodeType.Entry);
        foreach (var dialogue in list)
        {
            var node = new NodeReplicaData()
            {
                GUID = dialogue.GUID,
                text = dialogue.text,
                type = dialogue.NodeType,
                character = dialogue.character
            };
            if (dialogue.NodeType == NodeType.Dialog)
            {
                if (dialogue.outputContainer.childCount <= 2)
                {

                    node.classname = ClassName.NodeAutoReplicaData;
                }
                else
                {
                    node.classname = ClassName.NodeMultiReplicaData;
                }
            }
            else if (dialogue.NodeType == NodeType.Action)
            {
                node.action = dialogue.actions;
                node.classname = ClassName.NodeTriggerData;
            }
            else if (dialogue.NodeType == NodeType.End)
            {
                node.classname = ClassName.NodeEndData;
            }

            chain.Add(node);
        }

        if (!string.IsNullOrEmpty(entryGUID))
        {
            var entry = chain.Find(x => x.GUID == entryGUID);
            chain.RemoveAll(x => x.GUID == entryGUID);
            chain.Insert(0, entry);
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
                    if (multiReplica.classname == ClassName.NodeMultiReplicaData)
                    {
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
                    else if (multiReplica.classname == ClassName.NodeAutoReplicaData)
                    {
                        var port = dialogue.outputContainer.ElementAt(0) as Port;
                        if (port.connected)
                        {
                            var connectedGUID = (port.connections.ToList()[0].input.node as ExtendedNode).GUID;
                            multiReplica.nextGUID = connectedGUID;
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
    private void ClearGraph(ExtendedDialog file)
    {
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
