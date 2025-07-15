using UnityEngine;

[System.Serializable]
public class DialogueEventSO : ScriptableObject
{
    public virtual void RunEvent()
    {
        Debug.Log("Event called");
    }
}
