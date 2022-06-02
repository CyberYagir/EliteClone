using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.DemiEditor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

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
            Entry = true
        };

        node.SetPosition(new Rect(new Vector2(200,200), nodeSize));

        var port = CreatePort(node, Direction.Output);
        port.portName = "Next Replica";
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

    public void CreateNode()
    {
        AddElement(CreateDialogNode());
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

        
        var button = new Button(() => AddChoicePort(node));
        button.text = "Add";
        node.titleContainer.Add(button);

        var editButton = new Button(() =>
        {
            EditTextWindow.Open();
            EditTextWindow.GetInstance().SetData(node.text, node.GUID);
            EditTextWindow.GetInstance().ChangeCallback.AddListener(delegate(string str)
            {
                node.text = str;
                node.title = str;
            });
            EditTextWindow.GetInstance().ShowModal();
        });
        editButton.text = "Edit";
        node.titleContainer.Add(editButton);
        
        node.SetPosition(new Rect(nodeSize, pos == Vector2.zero ? new Vector2(200,200) : pos));
        node.RefreshExpandedState();
        node.RefreshPorts();

        return node;
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
            EditTextWindow.GetInstance().SetData(port.portName, node.GUID);
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
        node.RefreshExpandedState();
        node.RefreshPorts();
    }
}
