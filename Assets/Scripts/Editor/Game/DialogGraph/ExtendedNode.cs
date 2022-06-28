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
    public Label inputUELabel;
    public Button HeaderAddUIEl, HeaderEditUIEl;


    public void UpdateText(string newText)
    {
        List<string> strs = new List<string>();
        string tmp = "";
        for (int i = 0; i < newText.Length; i++)
        {
            tmp += newText[i].ToString();
            if (tmp.Length > 20)
            {
                if (newText[i] == ' ' || newText[i] == '.')
                {
                    strs.Add(tmp);
                    tmp = "";
                }
            }
        }

        string final = "";
        for (int i = 0; i < strs.Count; i++)
        {
            final += strs[i] + "\n";
        }

        inputUELabel.text = final;
    }
}
