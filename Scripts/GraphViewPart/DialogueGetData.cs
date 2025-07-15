using UnityEngine;

public class DialogueGetData : MonoBehaviour
{
    [SerializeField] protected DialogueContainerSO dialogueContainer;

    protected BaseNodeData GetNodeByGuid(string targetNodeGuid)
    {
        return dialogueContainer.AllNodes.Find(node => node.NodeGuid == targetNodeGuid);
    }

    protected BaseNodeData GetNodeByNodePort(DialogueNodePort nodePort)
    {
        return dialogueContainer.AllNodes.Find(port => port.NodeGuid == nodePort.InputGuid);
    }

    protected BaseNodeData GetNextNode(BaseNodeData baseNodeData)
    {
        NodeLinkData nodeLinkData = dialogueContainer.NodeLinkDatas.Find(edge => edge.BaseNodeGuid == baseNodeData.NodeGuid);

        return GetNodeByGuid(nodeLinkData.TargetedNodeGuid);
    }

    protected BaseNodeData GetNextNodeByIndex(BaseNodeData baseNodeData, int index)
    {
        NodeLinkData nodeLinkData = dialogueContainer.NodeLinkDatas.Find(edge => edge.BaseNodeGuid == baseNodeData.NodeGuid);

        return GetNodeByGuid(nodeLinkData.TargetedNodeGuid);
    }
}
