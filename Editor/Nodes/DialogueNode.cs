using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueNode : BaseNode
{
    private List<DialogueLine> dialogueLines = new List<DialogueLine>();
    private List<DialogueNodePort> dialogueNodePorts = new List<DialogueNodePort>();
    private Port defaultOutputPort = null;
    public List<DialogueNodePort> DialogueNodePorts { get => dialogueNodePorts; set => dialogueNodePorts = value; }
    public List<DialogueLine> DialogueLines { get => dialogueLines; }
    public Port DefaultOutputPort { get => defaultOutputPort; set => defaultOutputPort = value; }

    public DialogueNode()
    {

    }
    public DialogueNode(Vector2 pos, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        this.editorWindow = editorWindow;
        this.graphView = graphView;

        title = "Dialogue";
        SetPosition(new Rect(pos, new Vector2(200, 350)));
        nodeGuid = Guid.NewGuid().ToString();

        AddInputPort("Input", Port.Capacity.Multi);

        AddToClassList("dialogueNode");

        // Add port
        Button button = new Button() {
            text = "Add Choice",
        };
        button.clicked += () => {
            AddChoicePort(this);
        };

        titleButtonContainer.Add(button);

        // Add DialogueLine
        Button buttonLine = new Button() {
            text = "Add Dialogue Line",
        };
        buttonLine.clicked += () => {
            AddDialogueLine(this);
        };
        
        extensionContainer.Add(buttonLine);

        if (defaultOutputPort == null)
        {
            defaultOutputPort = AddOutputPort("Output");
        }

        // Refresh
        this.RefreshPorts();
        this.RefreshExpandedState();
    }

    public void AddDialogueLine(BaseNode baseNode, DialogueLine _dialogueLine = null)
    {
        // visualElement - container for data
        VisualElement visualElement = new VisualElement();
        DialogueLine dialogueLine = new DialogueLine();

        dialogueLine.DialogueLineID = Guid.NewGuid().ToString();

        if (_dialogueLine != null)
        {
            dialogueLine.Name = _dialogueLine.Name;
            dialogueLine.Sentence = _dialogueLine.Sentence;
            dialogueLine.AudioClip = _dialogueLine.AudioClip;
            dialogueLine.DialogueLineID = _dialogueLine.DialogueLineID;
        }

        // Name Label
        Label label_name = new Label("Name");
        visualElement.Add(label_name);

        // Name FIELD
        dialogueLine.NameField = new TextField();
        dialogueLine.NameField.RegisterValueChangedCallback(x => dialogueLine.Name = x.newValue);
        dialogueLine.NameField.SetValueWithoutNotify(dialogueLine.Name);
        dialogueLine.NameField.AddToClassList("Textname");
        visualElement.Add(dialogueLine.NameField);

        // Sentence Label
        Label label_sentence = new Label("Sentence box");
        visualElement.Add(label_sentence);

        // Sentence FIELD
        dialogueLine.SentenceField = new TextField();
        dialogueLine.SentenceField.RegisterValueChangedCallback(x => dialogueLine.Sentence = x.newValue);
        dialogueLine.SentenceField.SetValueWithoutNotify(dialogueLine.Sentence);
        dialogueLine.SentenceField.AddToClassList("TextBox");
        dialogueLine.SentenceField.multiline = true;
        visualElement.Add(dialogueLine.SentenceField);

        // AudioClip FIELD
        dialogueLine.AudioClipField = new ObjectField();
        dialogueLine.AudioClipField = new ObjectField() {
            objectType = typeof(AudioClip),
            allowSceneObjects = false,
            value = dialogueLine.AudioClip,
        };
        dialogueLine.AudioClipField.RegisterValueChangedCallback(value => dialogueLine.AudioClip = value.newValue as AudioClip);
        dialogueLine.AudioClipField.SetValueWithoutNotify(dialogueLine.AudioClip);
        visualElement.Add(dialogueLine.AudioClipField);

        //Delete Button
        Button deleteButton = new Button(() =>
        {
            DeleteDialogueLine(baseNode, visualElement, dialogueLine);
        })
        {
            text = "Delete",
        };

        visualElement.Add(deleteButton);

        baseNode.mainContainer.Add(visualElement);
        dialogueLines.Add(dialogueLine);
    }
    private void DeleteDialogueLine(BaseNode baseNode,VisualElement visualElement, DialogueLine _dialogueLine)
    {
        DialogueLine _ = dialogueLines.Find(x => x.DialogueLineID == _dialogueLine.DialogueLineID);
        dialogueLines.Remove(_);

        baseNode.mainContainer?.Remove(visualElement);
    }
    public override void LoadValueInToField()
    {
        foreach (DialogueLine dialogueLine in dialogueLines)
        {
            dialogueLine.NameField.SetValueWithoutNotify(dialogueLine.Name);
            dialogueLine.SentenceField.SetValueWithoutNotify(dialogueLine.Sentence);
            dialogueLine.AudioClipField.SetValueWithoutNotify(dialogueLine.AudioClip);
        }
    }
    public Port AddChoicePort(BaseNode baseNode, DialogueNodePort _dialogueNodePort = null)
    {
        Port port = GetPortInstance(Direction.Output);

        string outputPortName = $"Choice ";

        DialogueNodePort dialogueNodePort = new DialogueNodePort();
        dialogueNodePort.PortGuid = Guid.NewGuid().ToString();

        dialogueNodePort.Text = outputPortName;

        if (_dialogueNodePort != null)
        {
            dialogueNodePort.InputGuid = _dialogueNodePort.InputGuid;
            dialogueNodePort.OutputGuid = _dialogueNodePort.OutputGuid;
            dialogueNodePort.PortGuid = _dialogueNodePort.PortGuid;

            dialogueNodePort.Text = _dialogueNodePort.Text;
        }

        dialogueNodePort.TextField = new TextField();
        dialogueNodePort.TextField.RegisterValueChangedCallback(x => dialogueNodePort.Text = x.newValue);
        dialogueNodePort.TextField.SetValueWithoutNotify(dialogueNodePort.Text);
        port.contentContainer.Add(dialogueNodePort.TextField);

        //Delete Button
        Button deleteButton = new Button(() => DeletePort(baseNode, port))
        {
            text = "-",
        };

        port.contentContainer.Add(deleteButton);


        dialogueNodePort.MyPort = port;
        port.portName = "";

        dialogueNodePorts.Add(dialogueNodePort);

        baseNode.outputContainer.Add(port);

        // Delete Default Port
        if (defaultOutputPort != null)
        {
            DeletePort(baseNode, defaultOutputPort);
            defaultOutputPort = null;
        }

        // Refresh
        baseNode.RefreshPorts();
        baseNode.RefreshExpandedState();

        return port;
    }
    
    private void DeletePort(BaseNode node, Port port)
    {
        DialogueNodePort _ = dialogueNodePorts.Find(x => x.MyPort == port);
        dialogueNodePorts.Remove(_);

        IEnumerable<Edge> portEdge = graphView.edges.ToList().Where(edge => edge.output == port);

        if (portEdge.Any())
        { 
            Edge edge = portEdge.First();
            edge.input.Disconnect(edge);
            edge.output.Disconnect(edge);
            graphView.RemoveElement(edge);
        }

        node.outputContainer.Remove(port);

        // Delete Default Port
        if (outputContainer.childCount == 0)
        {
            defaultOutputPort = AddOutputPort("Output");
        }

        // Refresh
        node.RefreshPorts();
        node.RefreshExpandedState();
    }
    /// <summary>
    /// Pseudo:
    /// onInit => appear if outputContainer is empty
    /// onAdd => Delete port
    /// onDelete => if outputContainer is empty, add port
    /// </summary>
    public void AddDefaultOutputPortIfEmpty()
    {
        if (outputContainer.childCount == 0)
        {
            defaultOutputPort = AddOutputPort("Output");
        }
    }
}
