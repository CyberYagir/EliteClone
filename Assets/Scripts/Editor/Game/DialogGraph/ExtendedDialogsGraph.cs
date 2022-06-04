using System;
using System.Collections;
using System.Collections.Generic;
using Core.Dialogs;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class ExtendedDialogsGraph : EditorWindow
{

    
    
    public ExtendedDialogGraphView view;

    public string fileName;
    public ExtendedDialog file;
    
    [MenuItem("Tools/Graphs/Dialog Graph")]
    public static void OpenWindow()
    {
        var window = GetWindow<ExtendedDialogsGraph>();
        window.titleContent = new GUIContent("Extended Dialogs Editor");
    }


    public static ExtendedDialogsGraph GetGraph()
    {
        return GetWindow<ExtendedDialogsGraph>();
    }
    
    private void OnEnable()
    {
        ConstructGraph();
        CreateTools();
    }

    private void OnDisable()
    {
        EditTextWindow.Close();
        rootVisualElement.Remove(view);
    }

    public void ConstructGraph()
    {
        view = new ExtendedDialogGraphView();
        view.StretchToParentSize();
        rootVisualElement.Add(view);
    }

    public void CreateTools()
    {
        var toolbar = new Toolbar();
        CreateAddButton(toolbar);
        CreateAddButtonEnd(toolbar);
        CreateAddButtonAction(toolbar);
        CreateSaveFile(toolbar);
        rootVisualElement.Add(toolbar);
    }

    public void CreateSaveFile(Toolbar toolbar)
    {
        toolbar.Add(new Button(() => { DataOperation(true);}){text = "Save"});
    }
    
    public void CreateAddButton(Toolbar toolbar)
    {
        var createNodeB = new Button(delegate
        {
            CreateDialog(NodeType.Dialog);
        });
        createNodeB.text = "Add Replica";
        toolbar.Add(createNodeB);
    }
    
    public void CreateAddButtonEnd(Toolbar toolbar)
    {
        var createNodeB = new Button(delegate
        {
            CreateDialog(NodeType.End);
        });
        createNodeB.text = "Add end";
        toolbar.Add(createNodeB);
    }

    public void CreateAddButtonAction(Toolbar toolbar)
    {
        var createNodeB = new Button(delegate
        {
            CreateDialog(NodeType.Action);
        });
        createNodeB.text = "Add Action";
        toolbar.Add(createNodeB);
    }
    
    public void CreateDialog(NodeType n)
    {
        view.CreateNode(n);
    }

    public void SetFile(ExtendedDialog dialog)
    {
        file = dialog;
        fileName = AssetDatabase.GetAssetPath(file);
    }
    public void DataOperation(bool isSave)
    {
        if (!string.IsNullOrEmpty(fileName) && file != null)
        {
            var save = GraphSaveUtility.GetInstance(view);
            if (isSave)
            {
                save.SaveGraph(file);
            }
            else
            {
                save.LoadGraph(file);
            }
        }
    }
}
