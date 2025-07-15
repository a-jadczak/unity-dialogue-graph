using System;
using System.Collections.Generic;
using UnityEngine;

public class EventDispatcher
{
    private static EventDispatcher _instance;
    public static EventDispatcher Instance => _instance ??= new EventDispatcher();

    private readonly Dictionary<string, Delegate> _listeners = new();

    public void Subscribe(string key, Delegate callback) 
    {
        if (_listeners.ContainsKey(key))
        {
            Debug.LogWarning($"Event with key: '{key}' - already exist.");
            return;
        }

        _listeners.Add(key, callback);
    }

    public void Unsubscribe(string key)
    {
        _listeners.Remove(key);
    }

    public void Dispatch(string key)
    {
        if (_listeners.TryGetValue(key, out var cb))
        {
            cb.DynamicInvoke();
        }
    }
}
