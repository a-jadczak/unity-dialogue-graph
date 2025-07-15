using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    private event Action tempEvent;
    public static GameEvents Instance { get; private set; }

    public Action TempEvent { get => tempEvent; set => tempEvent = value; }

    private void Awake()
    {
        Instance = this;
    }

    public void CallTempEvent()
    {
        tempEvent?.Invoke();
    }

    
}
