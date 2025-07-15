using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialogueUI;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI textName;
    [SerializeField] private TextMeshProUGUI textBox;

    [System.Serializable]
    private struct MyButton {
        public Button button;
        public TextMeshProUGUI text;
    }

    [Header("Buttons")]
    [SerializeField] private MyButton button01;
    [SerializeField] private MyButton button02;
    [SerializeField] private MyButton button03;

    private List<MyButton> buttons = new List<MyButton>();

    private void Awake()
    {
        ShowDialogueUI(false);

        buttons.Add(button01);
        buttons.Add(button02);
        buttons.Add(button03);
    }

    public void ShowDialogueUI(bool boolean)
    {
        dialogueUI.SetActive(boolean);
    }

    public void SetText(string name, string textBox)
    {
        this.textName.text = name; 
        this.textBox.text = textBox;
    }

    public void SetButtons(List<string> texts, List<UnityAction> unityActions)
    {
        buttons.ForEach(myBtn => myBtn.button.gameObject.SetActive(false));
        float length = texts.Count;

        for (int i = 0; i < length; i++)
        {
            buttons[i].text.text = texts[i];
            buttons[i].button.gameObject.SetActive(true);
            buttons[i].button.onClick = new Button.ButtonClickedEvent();
            buttons[i].button.onClick.AddListener(unityActions[i]);
        }
    }
    public void HideButtons()
    {
        foreach (var item in buttons)
        {
            item.button.gameObject.SetActive(false);
        }
    }
}
