using System;
using UnityEngine;

public class DispatcherTester : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        Action test = () => { Debug.Log("<color=red>Hello!</color>"); };

        //EventDispatcher.Instance.Subscribe("TempEvent", test);
        BooleanDispatcher.Instance.Subscribe("TempBoolean", false);
        BooleanDispatcher.Instance.Save();
    }

    private void OnDestroy()
    {
        //EventDispatcher.Instance.Unsubscribe("TempEvent");
        BooleanDispatcher.Instance.Unsubscribe("TempBoolean");
    }
}