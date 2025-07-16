using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseNode : Node
{
    protected string nodeGuid;
    protected DialogueGraphView graphView;
    protected DialogueEditorWindow editorWindow;
    protected Vector2 defaultNodeSize = new Vector2(200, 250);
    protected string STYLE_NAME = "nodeStyle";
    public string NodeGuid { get => nodeGuid; set => nodeGuid = value;}

    public BaseNode()
    {
        StyleSheet styleSheet = Resources.Load<StyleSheet>(STYLE_NAME);
        styleSheets.Add(styleSheet);
    }

    public Port AddOutputPort(string name, Port.Capacity capacity = Port.Capacity.Single)
    {
        Port outputPort = GetPortInstance(Direction.Output, capacity);
        outputPort.portName = name;
        outputContainer.Add(outputPort);

        return outputPort;
    }

    public void AddInputPort(string name, Port.Capacity capacity = Port.Capacity.Multi)
    {
        Port inputPort = GetPortInstance(Direction.Input, capacity);
        inputPort.portName = name;
        inputContainer.Add(inputPort);
    }

    protected Port GetPortInstance(Direction nodeDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return InstantiatePort(Orientation.Horizontal, nodeDirection, capacity, typeof(float));
    }

    public virtual void LoadValueInToField()
    {

    }

}
