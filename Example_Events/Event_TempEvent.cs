using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Temp Event")]
public class Event_TempEvent : DialogueEventSO
{
    public override void RunEvent()
    {
        GameEvents.Instance.CallTempEvent();
    }
}
