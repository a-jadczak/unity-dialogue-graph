using UnityEngine;

public class TestDialogueUpdate : MonoBehaviour
{

    private DialogueTalk dialogueTalk;

    private void Awake()
    {
        dialogueTalk = GetComponent<DialogueTalk>();
    }

    private void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        if (Input.GetKeyDown(KeyCode.M))
        {
            dialogueTalk.StartDialogue();
        }
    }

}
