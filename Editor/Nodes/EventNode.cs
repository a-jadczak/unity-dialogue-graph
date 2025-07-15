using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class EventNode : BaseNode
{
    private DialogueEventSO dialogueEvent;
    private ObjectField objectField;
    public DialogueEventSO DialogueEvent { get => dialogueEvent; set => dialogueEvent = value; }

    public EventNode()
    {

    }

    public EventNode(Vector2 pos, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        this.editorWindow = editorWindow;
        this.graphView = graphView;


        title = "Event";
        SetPosition(new Rect(pos, defaultNodeSize) );
        nodeGuid = Guid.NewGuid().ToString();

        this.AddToClassList("eventNode");

        AddInputPort("Input", Port.Capacity.Multi);
        AddOutputPort("Output", Port.Capacity.Single);

        objectField = new ObjectField()
        {
            objectType = typeof(DialogueEventSO),
            //allowSceneObjects = false, // FIXME: normalnie bylo tak ale uznalem ze moze nie Xd
            value = dialogueEvent
        };

        objectField.RegisterValueChangedCallback(value =>
        {
            dialogueEvent = objectField.value as DialogueEventSO;
        });

        objectField.SetValueWithoutNotify(dialogueEvent);


        mainContainer.Add(objectField);
    }

    public override void LoadValueInToField()
    {
        objectField.SetValueWithoutNotify(dialogueEvent);
    }
}
