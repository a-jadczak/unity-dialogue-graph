using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTalk : DialogueGetData
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private AudioSource audioSource;

    private DialogueNodeData currentDialogueNodeData;
    private DialogueNodeData lastDialogueNodeData;

    [SerializeField] private float DIALOGUE_READ_DELAY = .5f;
    private bool isDialogueStarted = false;

    private void Awake()
    {
        dialogueUI = FindObjectOfType<DialogueUI>();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartDialogue()
    {
        if (isDialogueStarted)
        {
            return;
        }
        isDialogueStarted = true;

        CheckNodeType(GetNextNode(dialogueContainer.StartNodeDatas[0]));
        dialogueUI.ShowDialogueUI(true);
    }
    private void EndDialogue()
    {
        dialogueUI.ShowDialogueUI(false);
        isDialogueStarted = false;
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
            dialogueUI.HideButtons();
            foreach (DialogueLine dialogueLine in list)
            {
                dialogueUI.SetText(dialogueLine.Name, dialogueLine.Sentence);
                float sentanceLength;

                if (dialogueLine.AudioClip != null)
                {
                    audioSource.clip = dialogueLine.AudioClip;
                    audioSource.Play();
                    sentanceLength = dialogueLine.AudioClip.length;
                }
                else
                {
                    sentanceLength = dialogueLine.Sentence.EstimateReadingTime();
                }

                yield return new WaitForSeconds(sentanceLength + DIALOGUE_READ_DELAY);
            }
            // If there is no options
            if (nodeData.DialogueNodePorts.Count == 0)
            {
                CheckNodeType(GetNodeByGuid(nodeData.DefaultOutputPortGuidNode));
            }
            else
            {
                SetAndShowButtons(nodeData.DialogueNodePorts);
            }
        }

    }
    private void RunNode(EventNodeData nodeData)
    {
        if (nodeData.EventKey != string.Empty)
            EventDispatcher.Instance.Dispatch(nodeData.EventKey);
        else
            Debug.LogWarning("Event key is empty");

        CheckNodeType(GetNextNode(nodeData));
    }
    private void RunNode(EndNodeData nodeData)
    {
        switch (nodeData.EndNodeType)
        {
            case EndNodeType.End:
                EndDialogue();
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
        List<Action> actions = new List<Action>();

        foreach (DialogueNodePort port in nodePorts)
        {
            var localPort = port;
            texts.Add(localPort.Text);
            actions.Add(() =>
            {
                CheckNodeType(GetNodeByGuid(localPort.InputGuid));
            });
        }

        dialogueUI.SetButtons(texts, actions);
    }
}
