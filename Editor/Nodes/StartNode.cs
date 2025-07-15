using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class StartNode : BaseNode
{
    public StartNode()
    {

    }

    public StartNode(Vector2 position, DialogueEditorWindow editorWindow, DialogueGraphView graphView)
    {
        this.editorWindow = editorWindow;
        this.graphView = graphView;

        title = "Start";
        SetPosition(new Rect(position, defaultNodeSize));
        nodeGuid = Guid.NewGuid().ToString();

        this.AddToClassList("startNode");

        AddOutputPort("Output", Port.Capacity.Single);

        RefreshExpandedState();
        RefreshPorts();
    }

}
