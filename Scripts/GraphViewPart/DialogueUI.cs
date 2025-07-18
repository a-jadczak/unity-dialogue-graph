using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class DialogueUI : MonoBehaviour
{
    public UIDocument uiDocument;

    private Label nicknameLabel;
    private Label dialogLineLabel;

    private Button answerButton1;
    private Button answerButton2;
    private Button answerButton3;

    private Dictionary<Button, Action> answerButtons;

    void Awake()
    {
        var root = uiDocument.rootVisualElement;

        nicknameLabel = root.Q<Label>("nickname-label");
        dialogLineLabel = root.Q<Label>("dialog-line-label");
        answerButton1 = root.Q<Button>("answer-button-1");
        answerButton2 = root.Q<Button>("answer-button-2");
        answerButton3 = root.Q<Button>("answer-button-3");

        answerButtons = new Dictionary<Button, Action>()
        {
            { answerButton1, null },
            { answerButton2, null },
            { answerButton3, null }
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
        for (int i = 0; i < answerButtons.Count; i++)
        {
            var e = answerButtons.ElementAt(i);
            e.Key.clicked -= e.Value; // Remove previous actions
        }

        float length = texts.Count;

        for (int i = 0; i < length; i++)
        {
            var btn = answerButtons.ElementAt(i).Key;
            btn.text = texts[i];
            btn.visible = true;

            btn.clicked += actions[i];

            answerButtons[btn] = actions[i]; // Update the action for the button
        }
    }
    public void HideButtons()
    {
        foreach (var item in answerButtons)
        {
            item.Key.visible = false;
        }
    }
}
