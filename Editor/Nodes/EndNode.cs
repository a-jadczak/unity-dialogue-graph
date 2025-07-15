using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EndNode : BaseNode
{
    private EndNodeType endNodeType = EndNodeType.End;
    public EndNodeType EndNodeType { get => endNodeType; set => endNodeType = value; }
    
    private EnumField enumField;


    public EndNode()
    {

    }

    public EndNode(Vector2 pos, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        this.editorWindow = editorWindow;
        this.graphView = graphView;

        title = "End";
        SetPosition(new Rect(pos, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        this.AddToClassList("endNode");

        AddInputPort("Input", Port.Capacity.Multi);

        enumField = new EnumField()
        {
            value = endNodeType,
        };

        enumField.Init(endNodeType);

        enumField.RegisterValueChangedCallback( (value) => 
        {
            endNodeType = (EndNodeType)value.newValue;
        });

        enumField.SetValueWithoutNotify(endNodeType);

        mainContainer.Add(enumField);
    }

    public override void LoadValueInToField()
    {
        enumField.SetValueWithoutNotify(endNodeType);
    }

}
