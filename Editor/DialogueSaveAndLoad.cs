using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

public class DialogueSaveAndLoad
{
    private List<Edge> edges => graphView.edges.ToList();
    private List<BaseNode> nodes => graphView.nodes.ToList().Where(node => node is BaseNode).Cast<BaseNode>().ToList();

    private DialogueGraphView graphView;

    public DialogueSaveAndLoad(DialogueGraphView graphView)
    {
        this.graphView = graphView;
    }

    public void Save(DialogueContainerSO dialogueContainerSO)
    {
        SaveEdges(dialogueContainerSO);
        SaveNodes(dialogueContainerSO);

        //Tell unity data has been changed
        EditorUtility.SetDirty(dialogueContainerSO);
        //Save all the assets
        AssetDatabase.SaveAssets();
    }
    public void Load(DialogueContainerSO dialogueContainerSO)
    {
        ClearGraph();
        GenerateNodes(dialogueContainerSO);
        ConnectNodes(dialogueContainerSO);
    }

    #region Save Node Data Methods
    private void SaveEdges(DialogueContainerSO dialogueContainerSO)
    {
        dialogueContainerSO.NodeLinkDatas.Clear();

        Edge[] connectedEdges = edges.Where(edge => edge.input.node != null).ToArray();

        for (int i = 0; i < connectedEdges.Count(); i++)
        {
            BaseNode outputNode = (BaseNode) connectedEdges[i].output.node;
            BaseNode inputNode = connectedEdges[i].input.node as BaseNode;

            dialogueContainerSO.NodeLinkDatas.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.NodeGuid,
                TargetedNodeGuid = inputNode.NodeGuid,
            });
        }
    }
    private void SaveNodes(DialogueContainerSO dialogueContainerSO)
    {
        dialogueContainerSO.DialogueNodeDatas.Clear();
        dialogueContainerSO.StartNodeDatas.Clear();
        dialogueContainerSO.EndNodeDatas.Clear();
        dialogueContainerSO.EventNodeDatas.Clear();
        dialogueContainerSO.BranchNodeDatas.Clear();

        nodes.ForEach(node =>
        {
            switch (node)
            {
                case DialogueNode dialogueNode:
                    dialogueContainerSO.DialogueNodeDatas.Add(SaveNodeData(dialogueNode));
                    break;
                case StartNode startNode:
                    dialogueContainerSO.StartNodeDatas.Add(SaveNodeData(startNode));
                    break;
                case EndNode endNode:
                    dialogueContainerSO.EndNodeDatas.Add(SaveNodeData(endNode));
                    break;
                case EventNode eventNode:
                    dialogueContainerSO.EventNodeDatas.Add(SaveNodeData(eventNode));
                    break;
                case BranchNode branchNode:
                    dialogueContainerSO.BranchNodeDatas.Add(SaveNodeData(branchNode));
                    break;
                default:
                    break;
            }
        });
    }

    private DialogueNodeData SaveNodeData(DialogueNode dialogueNode)
    {
        DialogueNodeData dialogueNodeData = new DialogueNodeData()
        {
            NodeGuid = dialogueNode.NodeGuid,
            Position = dialogueNode.GetPosition().position,

            DialogueLines = new List<DialogueLine>(dialogueNode.DialogueLines),
            DialogueNodePorts = new List<DialogueNodePort>(dialogueNode.DialogueNodePorts),
            DefaultOutputPortGuidNode = string.Empty,
        };


        foreach (DialogueNodePort nodePort in dialogueNodeData.DialogueNodePorts)
        {
            nodePort.OutputGuid = string.Empty;
            nodePort.InputGuid = string.Empty;


            foreach (Edge edge in edges)
            {
                if (edge.output == nodePort.MyPort)
                {
                    nodePort.OutputGuid = (edge.output.node as BaseNode).NodeGuid;
                    nodePort.InputGuid = (edge.input.node as BaseNode).NodeGuid;
                }
            }
        }

        Debug.Log($"DefaultOutputPort: {dialogueNodeData.DefaultOutputPortGuidNode}");

        Edge defaultOutput = edges
            .Where(x => x.output.node == dialogueNode)
            .Cast<Edge>()
            .FirstOrDefault(x => x.output.portName == "Output");

        if (defaultOutput != null)
            dialogueNodeData.DefaultOutputPortGuidNode = (defaultOutput.input.node as BaseNode).NodeGuid;


        return dialogueNodeData;
    }
    private StartNodeData SaveNodeData(StartNode startNode)
    {
        return new StartNodeData() 
        {
            NodeGuid = startNode.NodeGuid,
            Position = startNode.GetPosition().position 
        };
    }
    private EndNodeData SaveNodeData(EndNode endNode)
    {
        return new EndNodeData()
        {
            NodeGuid = endNode.NodeGuid,
            Position = endNode.GetPosition().position,
            EndNodeType = endNode.EndNodeType
        };
    }
    private EventNodeData SaveNodeData(EventNode eventNode)
    {
        return new EventNodeData()
        {
            NodeGuid = eventNode.NodeGuid,
            Position = eventNode.GetPosition().position,
            EventKey = eventNode.EventKey,
        };
    }
    private BranchNodeData SaveNodeData(BranchNode branchNode)
    {
        var tmpEdges = edges.Where(x => x.output.node == branchNode).Cast<Edge>().ToList();

        Edge trueOutput = tmpEdges.FirstOrDefault(x => x.output.portName == "True");
        Edge flaseOutput = tmpEdges.FirstOrDefault(x => x.output.portName == "False");


        return new BranchNodeData()
        {
            NodeGuid = branchNode.NodeGuid,
            Position = branchNode.GetPosition().position,
            BooleanSO = branchNode.BooleanSO,
            TrueGuidNode = (trueOutput != null ? (trueOutput.input.node as BaseNode).NodeGuid : string.Empty),
            FalseGuidNode = (flaseOutput != null ? (flaseOutput.input.node as BaseNode).NodeGuid : string.Empty),
        };
    }

    #endregion

    #region Load Node Data Methods    
    private void ClearGraph()
    {
        edges.ForEach(edge => graphView.RemoveElement(edge));
        nodes.ForEach(node => graphView.RemoveElement(node));
    }
    private void GenerateNodes(DialogueContainerSO dialogueContainer)
    {
        // Start
        dialogueContainer.StartNodeDatas.ForEach(node =>
        {
            StartNode tempNode = graphView.CreateStartNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;

            graphView.AddElement(tempNode);
        });

        // End Node
        foreach (EndNodeData node in dialogueContainer.EndNodeDatas)
        {
            EndNode tempNode = graphView.CreateEndNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;
            tempNode.EndNodeType = node.EndNodeType;

            graphView.AddElement(tempNode);
        }

        // Event Node
        foreach (EventNodeData node in dialogueContainer.EventNodeDatas)
        {
            EventNode tempNode = graphView.CreateEventNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;
            tempNode.EventKey = node.EventKey;
            tempNode.LoadValueInToField();

            graphView.AddElement(tempNode);
        }

        // Dialogue Node
        foreach (DialogueNodeData node in dialogueContainer.DialogueNodeDatas)
        {
            DialogueNode tempNode = graphView.CreateDialogueNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;

            foreach (DialogueLine dialogueLine in node.DialogueLines)
            {
                tempNode.AddDialogueLine(tempNode, dialogueLine);
            }

            foreach (DialogueNodePort port in node.DialogueNodePorts)
            {
                tempNode.AddChoicePort(tempNode, port);
            }

            tempNode.AddDefaultOutputPortIfEmpty();

            tempNode.LoadValueInToField();
            graphView.AddElement(tempNode);
        }

        // Branch Node
        foreach (BranchNodeData node in dialogueContainer.BranchNodeDatas)
        {
            BranchNode tempNode = graphView.CreateBranchNode(node.Position);
            tempNode.NodeGuid = node.NodeGuid;
            tempNode.BooleanSO = node.BooleanSO;
            tempNode.LoadValueInToField();

            graphView.AddElement(tempNode);
        }
    }
    private void ConnectNodes(DialogueContainerSO dialogueContainer)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            List<NodeLinkData> connections = dialogueContainer.NodeLinkDatas
                .Where(edge => edge.BaseNodeGuid.Equals(nodes[i].NodeGuid))
                .ToList();

            for (int j = 0; j < connections.Count; j++)
            {
                string targetNodeGuid = connections[j].TargetedNodeGuid;
                BaseNode targetNode = nodes.First(node => node.NodeGuid == targetNodeGuid);

                if ((nodes[i] is DialogueNode) == false || (nodes[i] as DialogueNode).DefaultOutputPort != null)
                {
                    LinkNodesTogether(nodes[i].outputContainer[j].Q<Port>(), (Port)targetNode.inputContainer[0]);
                }
            }
        }
        
        List<DialogueNode> dialogueNodes = 
             nodes.FindAll(node => node is DialogueNode)
            .Cast<DialogueNode>()
            .ToList();

        foreach (DialogueNode dialogueNode in dialogueNodes)
        {
            foreach (DialogueNodePort nodePort in dialogueNode.DialogueNodePorts)
            {
                if (nodePort.InputGuid != string.Empty)
                {
                    BaseNode targetNode = nodes.First(Node => Node.NodeGuid == nodePort.InputGuid);
                    LinkNodesTogether(nodePort.MyPort, (Port)targetNode.inputContainer[0]);
                }
            }
        }
    }
    private void LinkNodesTogether(Port outputPort, Port inputPort)
    {
        Edge tempEdge = new Edge()
        {
            output = outputPort,
            input = inputPort,
        };
        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);

        graphView.Add(tempEdge);
    }
    #endregion
}
