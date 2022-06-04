using System.Collections;
using System.Collections.Generic;
using Core.Dialogs;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ExtendedNode : Node
{
    public string GUID;

    public string text;

    public NodeType NodeType;
    public Characters character;
    public Actions actions;
    
    
    public EnumField characterUIEl, actionUIEl;
    public Button HeaderAddUIEl, HeaderEditUIEl;

}
