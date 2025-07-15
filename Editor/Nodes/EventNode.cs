using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class EventNode : BaseNode
{
    private string _eventKey;
    private TextField textField;
    public string EventKey { get => _eventKey; set => _eventKey = value; }

    public EventNode()
    {

    }

    public EventNode(Vector2 pos, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        this.editorWindow = editorWindow;
        this.graphView = graphView;

        title = "Event";
        SetPosition(new Rect(pos, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        this.AddToClassList("eventNode");

        AddInputPort("Input", Port.Capacity.Multi);
        AddOutputPort("Output", Port.Capacity.Single);


        var label = new Label()
        {
            text = " Event key"
        };

        textField = new TextField()
        {
            value = _eventKey,
        };

        textField.RegisterValueChangedCallback(value =>
        {
            _eventKey = textField.value;
        });

        textField.SetValueWithoutNotify(_eventKey);

        mainContainer.Add(label);
        mainContainer.Add(textField);
    }

    public override void LoadValueInToField()
    {
        textField.SetValueWithoutNotify(_eventKey);
    }
}
