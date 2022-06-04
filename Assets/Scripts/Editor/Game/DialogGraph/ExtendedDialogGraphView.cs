using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Dialogs;
using DG.DemiEditor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Object = UnityEngine.Object;

public class ExtendedDialogGraphView : GraphView
{
    public static readonly Vector2 nodeSize = new Vector2(500, 100);
    public ExtendedDialogGraphView()
    {
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());


        AddElement(CreateFirstNode());
    }

    private ExtendedNode CreateFirstNode()
    {
        var node = new ExtendedNode()
        {
            title = "Start",
            GUID = GUID.Generate().ToString(),
            NodeType = NodeType.Entry
        };

        node.SetPosition(new Rect(new Vector2(200,200), nodeSize));

        var port = CreatePort(node, Direction.Output);
        port.portName = "Next Replica";
        port.portColor = Color.green;
        node.outputContainer.Add(port);
        node.RefreshExpandedState();
        node.RefreshPorts();
        return node;
    }

    public Port CreatePort(ExtendedNode node, Direction dir, Port.Capacity cap = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, dir, cap, typeof(string));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatible = new List<Port>();

        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                compatible.Add(port);
            }
        });

        return compatible;
    }

    
    public ExtendedNode CreateNode(NodeType nodeType)
    {
        ExtendedNode node = null;
        switch (nodeType)
        {
            case NodeType.Dialog:
                node = CreateDialogNode();
                break;
            case NodeType.End:
                node = CreateEndNode();
                break;
            case NodeType.Action:
                node = CreateActionNode();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nodeType), nodeType, null);
        }

        if (node != null)
        {
            AddElement(node);
        }

        return node;
    }


    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        if (evt.target is GraphView || evt.target is Node)
        {
            evt.menu.AppendAction("Create Replica Node", (e) => { CreateNode(NodeType.Dialog); });
            evt.menu.AppendAction("Create End Node", (e) => { CreateNode(NodeType.End); });
            evt.menu.AppendAction("Create Trigger Node", (e) => { CreateNode(NodeType.Action); });


            if (evt.target is Node)
            {
                var ex = evt.target as ExtendedNode;
                if (ex.NodeType != NodeType.Entry)
                {
                    evt.menu.AppendAction("Delete", (e) => { DeleteSelection(); });
                }

                if (ex.NodeType != NodeType.Action && ex.NodeType != NodeType.Entry)
                {
                    evt.menu.AppendAction("Edit", (e) => { EditButton(ex); });
                    if (ex.NodeType != NodeType.End)
                    {
                        evt.menu.AppendAction("Add", (e) => { AddButton(ex); });
                    }
                }
            }
        }
        evt.menu.AppendSeparator();
        evt.menu.AppendAction("Save", (e) => { ExtendedDialogsGraph.GetGraph().DataOperation(true); });

    }

    public ExtendedNode CreateActionNode(Vector2 pos = default)
    {
        var node = CreateDialogNode(pos);

        
        node.outputContainer.Clear();
        var inputPort = (node.inputContainer.ElementAt(0) as Port);
        inputPort.portType = typeof(int);
        inputPort.portName = "Trigger";
        node.NodeType = NodeType.Action;

        
        node.actionUIEl = new EnumField(node.actions);
        node.actionUIEl.RegisterValueChangedCallback(delegate(ChangeEvent<Enum> evt)
        {
            node.actions = (Actions)evt.newValue;
        });
        
        node.outputContainer.Add(node.actionUIEl);
        
        node.titleContainer.Q<EnumField>().visible = false;
        node.HeaderAddUIEl.visible = false;
        node.HeaderEditUIEl.visible = false;

        var color = (Color) new Color32(148, 129, 230, 255);
        node.titleContainer.style.backgroundColor = new StyleColor(color);
        
        node.RefreshExpandedState();
        node.RefreshPorts();
        
        return node;
    }
    public ExtendedNode CreateEndNode(Vector2 pos = default)
    {
        var node = CreateDialogNode(pos);

        node.HeaderAddUIEl.visible = false;
        node.outputContainer.Clear();
        var inputPort = (node.inputContainer.ElementAt(0) as Port);
        inputPort.portName = "End";

        
        var color = (Color) new Color32(165, 0, 52, 255);
        node.titleContainer.style.backgroundColor = new StyleColor(color);
        
        var actionPort = CreatePort(node, Direction.Output, Port.Capacity.Single);
        actionPort.portName = "Action";
        actionPort.portType = typeof(int);
        node.outputContainer.Add(actionPort);
        
        node.NodeType = NodeType.End;
        node.RefreshExpandedState();
        node.RefreshPorts();
        return node;
    }

    public ExtendedNode CreateDialogNode(Vector2 pos = default)
    {
        var node = new ExtendedNode()
        {
            title = "",
            text = "",
            GUID = GUID.Generate().ToString()
        };
        var input = CreatePort(node, Direction.Input, Port.Capacity.Multi);
        input.portName = "Prev";
        node.inputContainer.Add(input);
        
        node.NodeType = NodeType.Dialog;
        
        
        var color = (Color) new Color32(0, 165, 95, 255);
        node.titleContainer.style.backgroundColor = new StyleColor(color);
        
        node.characterUIEl = new EnumField(node.character);
        node.titleContainer.Add(node.characterUIEl);
        node.characterUIEl.RegisterValueChangedCallback(delegate(ChangeEvent<Enum> evt)
        {
            node.character = (Characters) evt.newValue;
        });
        
        
        node.HeaderAddUIEl = new Button(() => AddButton(node));
        node.HeaderAddUIEl.text = "Add";
        node.titleContainer.Add(node.HeaderAddUIEl);

        node.HeaderEditUIEl = new Button(() =>
        {
            EditButton(node);
        });
        node.HeaderEditUIEl.text = "Edit";
        node.titleContainer.Add(node.HeaderEditUIEl);


        var autoPort = CreatePort(node, Direction.Output, Port.Capacity.Single);
        autoPort.portType = typeof(string);
        autoPort.portName = "Auto";
        autoPort.portColor = Color.green;
        node.outputContainer.Add(autoPort);
        
        node.SetPosition(new Rect(nodeSize, pos == Vector2.zero ? new Vector2(200,200) : pos));
        node.RefreshExpandedState();
        node.RefreshPorts();

        return node;
    }

    public void AddButton(ExtendedNode node)
    {
        if (node.outputContainer.childCount == 1)
        {
            for (int i = 0; i < 2; i++)
            {
                AddChoicePort(node);
            }
        }
        else
        {
            AddChoicePort(node);
        }
    }
    public void EditButton(ExtendedNode node)
    {
        EditTextWindow.Open();
        EditTextWindow.GetInstance().SetData(node.text);
        EditTextWindow.GetInstance().ChangeCallback.AddListener(delegate(string str)
        {
            node.text = str;
            node.title = str;
        });
        EditTextWindow.GetInstance().ShowModal();
    }
    
    public void AddChoicePort(ExtendedNode node, string overrideName = "")
    {
        var port = CreatePort(node, Direction.Output, Port.Capacity.Single);
        port.contentContainer.Q<Label>("type").text = "";
        var outputContainer = node.outputContainer.Query("connector").ToList().Count;
        port.portName = "Option " + outputContainer;
        if (overrideName != "")
        {
            port.portName = overrideName;
        }
        
        var editButton = new Button(() =>
        {
            EditTextWindow.Open();
            EditTextWindow.GetInstance().SetData(port.portName);
            EditTextWindow.GetInstance().ChangeCallback.AddListener(delegate(string str)
            {
                port.portName = str;
            });
            EditTextWindow.GetInstance().ShowModal();

        })
        {
            text = "edit"
        };
        var deleteButton = new Button(() => RemovePort(node, port))
        {
            text = "-"
        };
        
        port.contentContainer.Add(deleteButton);
        port.contentContainer.Add(editButton);
        
        
        node.outputContainer.Add(port);

        
            
        var autoPort = node.outputContainer.ElementAt(node.outputContainer.childCount - 1) as Port;
        for (int i = 0; i < node.outputContainer.childCount; i++)
        {
            autoPort.SendToBack();
        }
        autoPort = node.outputContainer.ElementAt(node.outputContainer.childCount - 1) as Port;
        if (node.outputContainer.childCount > 1)
        {
            if (autoPort.connected)
            {
                var edge = autoPort.connections.ToList()[0];
                edge.input.Disconnect(edge);
                edge.output.Disconnect(edge);
                RemoveElement(edge);
                autoPort.DisconnectAll();
            }

            autoPort.SetEnabled(false);
        }
        else
        {
            autoPort.SetEnabled(true);
        }
        node.RefreshExpandedState();
        node.RefreshPorts();
    }

    private void RemovePort(ExtendedNode node, Port port)
    {
        var targetEdge = edges.ToList().Where(x => x.output.portName == port.portName && x.output.node == port.node);
        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }
        node.outputContainer.Remove(port);

        if (node.outputContainer.childCount == 2)
        {
            RemovePort(node, node.outputContainer.ElementAt(0) as Port);
            var autoPort = node.outputContainer.ElementAt(node.outputContainer.childCount - 1);
            autoPort.SetEnabled(true);
        }
        
        node.RefreshExpandedState();
        node.RefreshPorts();
    }
}
