using System;
using UnityEngine;

public class EventDispatcher : Dispatcher<Action>
{
    private static EventDispatcher _instance;
    public static EventDispatcher Instance => _instance ??= new EventDispatcher();
    public override Action Dispatch(string key)
    {
        if (_listeners.TryGetValue(key, out var cb))
        {
            cb.Invoke();
            return cb;
        }
        else
        {
            Debug.LogWarning($"Event with key: '{key}' - does not exist.");
            return null;
        }
    }
}
