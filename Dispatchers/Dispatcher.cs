using System.Collections.Generic;
using UnityEngine;

public abstract class Dispatcher<T>
{
    protected readonly Dictionary<string, T> _listeners = new();
    public virtual void Subscribe(string key, T value)
    {
        if (_listeners.ContainsKey(key))
        {
            Debug.LogWarning($"Value with key: '{key}' - already exist.");
            return;
        }

        _listeners.Add(key, value);
    }
    public virtual void Unsubscribe(string key)
    {
        _listeners.Remove(key);
    }
    public abstract T Dispatch(string key);
}
