using System.Collections.Generic;
using System.Linq;
using Core.Dialogs;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

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
            
            dialogContainer.nodesLinks.Add(new ExtendedDialog.NodeLink()
            {
                baseNodeGUID = output.GUID,
                portName = connectedPorts[i].output.portName,
                targetNodeGUID = input.GUID
            });
        }

        foreach (var dialogue in nodes.Where(node=>!node.Entry))
        {
            dialogContainer.nodeData.Add(new ExtendedDialog.NodeData()
            {
                nodeGUID = dialogue.GUID,
                text = dialogue.text,
                pos = dialogue.GetPosition().position
            });
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
            var tmpNode = target.CreateDialogNode(node.pos);
            tmpNode.GUID = node.nodeGUID;
            tmpNode.text = node.text;
            tmpNode.title = node.text;
            target.AddElement(tmpNode);

            
            var ports = file.nodesLinks.Where(x => x.baseNodeGUID == node.nodeGUID).ToList();

            ports.ForEach(x=>target.AddChoicePort(tmpNode, x.portName));

            tmpNode.RefreshPorts();
            tmpNode.RefreshExpandedState();
        }
    }

    private void ClearGraph(ExtendedDialog file)
    {
        nodes.Find(x => x.Entry).GUID = file.nodesLinks[0].baseNodeGUID;

        foreach (var node in nodes)
        {
            if (node.Entry) continue;
            edges.Where(x => x.input.node == node).ToList().ForEach(edge =>
            {
                target.RemoveElement(edge);
            });
            
            target.RemoveElement(node);
        }
    }
}
