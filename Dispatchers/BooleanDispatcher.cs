using UnityEngine;

public class BooleanDispatcher : Dispatcher<bool>
{
    private static BooleanDispatcher _instance;
    public static BooleanDispatcher Instance => _instance ??= new BooleanDispatcher();
    public override bool Dispatch(string key)
    {
        if (_listeners.TryGetValue(key, out var value))
            return value;

        Debug.LogWarning($"Boolean with key: '{key}' does not exist. Returning default false.");
        return false;
    }
}
