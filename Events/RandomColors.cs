using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomColors : MonoBehaviour
{
    private void Awake()
    {
        
    }

    private void Start()
    {
        Action test = () => { Debug.Log("<color=red>Hello!</color>"); };

        EventDispatcher.Instance.Subscribe("TempEvent", test);
    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.Unsubscribe("TempEvent");
    }



}
