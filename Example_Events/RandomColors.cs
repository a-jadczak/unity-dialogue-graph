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
        //GameEvents.Instance.TempEvent += DoTempEvent;    
    }

    private void OnDestroy()
    {
        //GameEvents.Instance.TempEvent -= DoTempEvent;
    }

    private void DoTempEvent()
    {
        Debug.Log("<color=blue>Siema</color>");
    }

}
