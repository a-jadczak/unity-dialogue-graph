using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTalk : DialogueGetData
{
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private AudioSource audioSource;

    private DialogueNodeData currentDialogueNodeData;
    private DialogueNodeData lastDialogueNodeData;

    [SerializeField] private float DIALOGUE_READ_DELAY = .5f;
    private void Awake()
    {
        dialogueController = FindObjectOfType<DialogueController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDialogue()
    {
        CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
        dialogueController.ShowDialogueUI(true);
    }
    private void CheckNodeType(BaseNodeData baseNodeData)
    {
        switch (baseNodeData)
        {
            case StartNodeData nodeData:
                RunNode(nodeData);
                break;
            case EndNodeData nodeData:
                RunNode(nodeData);
                break;
            case EventNodeData nodeData:
                RunNode(nodeData);
                break;
            case DialogueNodeData nodeData:
                RunNode(nodeData);
                break;
            case BranchNodeData nodeData:
                RunNode(nodeData);
                break;

            default:
                break;
        }
    }

    private void RunNode(StartNodeData nodeData)
    {
        CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
    }
    private void RunNode(DialogueNodeData nodeData)
    {
        if (currentDialogueNodeData != nodeData)
        {
            lastDialogueNodeData = currentDialogueNodeData;
            currentDialogueNodeData = nodeData;
        }

        StartCoroutine(DisplayDialogueLine(nodeData.DialogueLines));
                
        IEnumerator DisplayDialogueLine(List<DialogueLine> list)
        {
            dialogueController.HideButtons();
            foreach (DialogueLine dialogueLine in list)
            {
                dialogueController.SetText(dialogueLine.Name, dialogueLine.Sentence);
                float audioLength;
                if (dialogueLine.AudioClip != null)
                {
                    audioSource.clip = dialogueLine.AudioClip;
                    audioSource.Play();
                    audioLength = dialogueLine.AudioClip.length;
                }
                else
                {
                    audioLength = 2f;
                    Debug.LogWarning("You didn't set an Audio Clip !!!");
                }

                yield return new WaitForSeconds(audioLength + DIALOGUE_READ_DELAY);
            }
            SetAndShowButtons(nodeData.DialogueNodePorts);
        }

    }
    private void RunNode(EventNodeData nodeData)
    {
        nodeData?.DialogueEventSO.RunEvent();
        CheckNodeType(GetNextNode(nodeData));
    }
    private void RunNode(EndNodeData nodeData)
    {
        switch (nodeData.EndNodeType)
        {
            case EndNodeType.End:
                dialogueController.ShowDialogueUI(false);
                break;
            case EndNodeType.Repeat:
                CheckNodeType(GetNodeByGuid(currentDialogueNodeData.NodeGuid));
                break;
            case EndNodeType.Goback:
                CheckNodeType(GetNodeByGuid(lastDialogueNodeData.NodeGuid));
                break;
            case EndNodeType.ReturnToStart:
                break;
            default:
                break;
        }
    }
    private void RunNode(BranchNodeData nodeData)
    {
        bool boolean = nodeData.BooleanSO.BooleanValue;
        
        if (boolean)
        {
            CheckNodeType(GetNodeByGuid(nodeData.TrueGuidNode));
        }
        else
        {
            CheckNodeType(GetNodeByGuid(nodeData.FalseGuidNode));
        }
            
    }
    private void SetAndShowButtons(List<DialogueNodePort> nodePorts)
    {
        List<string> texts = new List<string>();
        List<UnityAction> unityActions = new List<UnityAction>();

        foreach (DialogueNodePort nodePort in nodePorts)
        {
            texts.Add(nodePort.Text);
            UnityAction tempAction = null;
            tempAction += () =>
            {
                CheckNodeType(GetNodeByGuid(nodePort.InputGuid));
            };
            unityActions.Add(tempAction);
        }

        dialogueController.SetButtons(texts, unityActions);
    }
}
