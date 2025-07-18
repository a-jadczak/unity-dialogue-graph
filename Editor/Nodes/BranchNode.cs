using System;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BranchNode : BaseNode
{
    private TextField booleanField;
    private string _booleanKey;

    public string BooleanKey { get => _booleanKey; set => _booleanKey = value; }

    public BranchNode()
    {

    }

    public BranchNode(Vector2 pos, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        this.editorWindow = editorWindow;
        this.graphView = graphView;

        title = "Branch";
        SetPosition(new Rect(pos, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        this.AddToClassList("branchNode");

        AddInputPort("Input", Port.Capacity.Multi);
        AddOutputPort("True", Port.Capacity.Single);
        AddOutputPort("False", Port.Capacity.Single);

        var label = new Label()
        {
            text = " Boolean key"
        };

        booleanField = new TextField()
        {
            value = _booleanKey,
        };


        booleanField.RegisterValueChangedCallback(value =>
        {
            _booleanKey = booleanField.value;
        });

        booleanField.SetValueWithoutNotify(_booleanKey);

        mainContainer.Add(label);
        mainContainer.Add(booleanField);

    }

    public void GetPort()
    {

    }
    public override void LoadValueInToField()
    {
        booleanField.SetValueWithoutNotify(_booleanKey);
    }
}
