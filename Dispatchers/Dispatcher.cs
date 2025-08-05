using GameSaveSystem;
public abstract class Dispatcher<T>
{
    protected SerializableDictionary<string, T> _listeners = new();
    public virtual void Subscribe(string key, T value)
    {
        _listeners[key] = value;
    }
    public virtual void Unsubscribe(string key)
    {
        _listeners.Remove(key);
    }
    public abstract T Dispatch(string key);
}
