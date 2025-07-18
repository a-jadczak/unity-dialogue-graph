using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
[System.Serializable]
public class DialogueContainerSO : ScriptableObject
{
    public List<NodeLinkData> NodeLinkDatas = new List<NodeLinkData>();
    
    public List<DialogueNodeData> DialogueNodeDatas = new List<DialogueNodeData>();
    public List<EndNodeData> EndNodeDatas = new List<EndNodeData>();
    public List<EventNodeData> EventNodeDatas = new List<EventNodeData>();
    public List<StartNodeData> StartNodeDatas = new List<StartNodeData>();
    public List<BranchNodeData> BranchNodeDatas = new List<BranchNodeData>();


    public List<BaseNodeData> AllNodes
    {
        get 
        {
            List<BaseNodeData> temp = new List<BaseNodeData>();
            temp.AddRange(DialogueNodeDatas);
            temp.AddRange(EndNodeDatas);
            temp.AddRange(EventNodeDatas);
            temp.AddRange(StartNodeDatas);
            temp.AddRange(BranchNodeDatas);

            return temp;
        }
    }

}

#region Nodes Data to Save
/// <summary>
/// Data to save, Links to Input, Outputs, Position, id (...)
/// </summary>
[System.Serializable]
public class NodeLinkData
{
    public string BaseNodeGuid;
    public string TargetedNodeGuid;
}

/// <summary>
/// Data to save, Position, id
/// </summary>
[System.Serializable]
public class BaseNodeData
{
    public string NodeGuid;
    public Vector2 Position;
}

[System.Serializable]
public class DialogueNodeData : BaseNodeData
{
    public List<DialogueNodePort> DialogueNodePorts;
    public List<DialogueLine> DialogueLines;
    public string DefaultOutputPortGuidNode;
}

[System.Serializable]
public class EndNodeData : BaseNodeData
{
    public EndNodeType EndNodeType;
}

[System.Serializable]
public class StartNodeData : BaseNodeData
{

}
[System.Serializable]
public class EventNodeData : BaseNodeData
{
    public string EventKey;
}
[System.Serializable]
public class BranchNodeData : BaseNodeData 
{
    public string BooleanKey;

    public string TrueGuidNode;
    public string FalseGuidNode;
}

#endregion

[System.Serializable]
public class DialogueNodePort
{
    public string PortGuid;
    public string InputGuid;
    public string OutputGuid;
    public Port MyPort;
    public TextField TextField;
    public string Text;
}

[System.Serializable]
public class DialogueLine
{
    public string Name;
    public string Sentence;
    public AudioClip AudioClip;

    public TextField NameField;
    public TextField SentenceField;
    public ObjectField AudioClipField;

    public string DialogueLineID;

}