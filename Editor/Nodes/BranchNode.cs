using System;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEditor.UIElements;

public class BranchNode : BaseNode
{
    private ObjectField booleanField;
    private BooleanSO booleanSO;

    public BooleanSO BooleanSO { get => booleanSO; set => booleanSO = value; }

    public BranchNode()
    {

    }

    public BranchNode(Vector2 pos, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        this.editorWindow = editorWindow;
        this.graphView = graphView;

        // x +=
        // x -=

        title = "Branch";
        SetPosition(new Rect(pos, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        this.AddToClassList("branchNode");

        AddInputPort("Input", Port.Capacity.Multi);
        AddOutputPort("True", Port.Capacity.Single);
        AddOutputPort("False", Port.Capacity.Single);

        Label labelBoolean = new Label("Boolean value");
        CreateLabel(labelBoolean, "Boolean value");

        booleanField = new ObjectField()
        {
            objectType = typeof(BooleanSO),
            value = BooleanSO
        };

        booleanField.RegisterValueChangedCallback(value =>
        {
            BooleanSO = booleanField.value as BooleanSO;
        });
        booleanField.SetValueWithoutNotify(BooleanSO);

        mainContainer.Add(booleanField);

    }

    public void GetPort()
    {

    }
    private void CreateLabel(Label label, string x)
    {
        label.AddToClassList("label_" + x);
        label.AddToClassList("Label");
        mainContainer.Add(label);
    }

    public override void LoadValueInToField()
    {
        booleanField.SetValueWithoutNotify(BooleanSO);
    }
}
