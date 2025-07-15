using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueUI : MonoBehaviour
{
    public UIDocument uiDocument;

    private Label nicknameLabel;
    private Label dialogLineLabel;

    private Button answerButton1;
    private Button answerButton2;
    private Button answerButton3;

    private List<Button> answerButtons;

    void Awake()
    {
        var root = uiDocument.rootVisualElement;

        nicknameLabel = root.Q<Label>("nickname-label");
        dialogLineLabel = root.Q<Label>("dialog-line-label");
        answerButton1 = root.Q<Button>("answer-button-1");
        answerButton2 = root.Q<Button>("answer-button-2");
        answerButton3 = root.Q<Button>("answer-button-3");

        answerButtons = new List<Button>()
        {
            answerButton1,
            answerButton2,
            answerButton3,
        };

        ShowDialogueUI(false);
    }

    public void ShowDialogueUI(bool boolean) => uiDocument.rootVisualElement.style.display = boolean ? DisplayStyle.Flex : DisplayStyle.None;

    public void SetText(string name, string textBox)
    {
        nicknameLabel.text = name;
        dialogLineLabel.text = textBox;
    }

    public void SetButtons(List<string> texts, List<Action> actions)
    {
        answerButtons.ForEach(myBtn => myBtn.visible = false);
        float length = texts.Count;

        for (int i = 0; i < length; i++)
        {
            answerButtons[i].text = texts[i];
            answerButtons[i].visible = true;
            answerButtons[i].clicked += actions[i];
        }
    }
    public void HideButtons()
    {
        answerButtons.ForEach(btn => btn.visible = false);
    }
}
