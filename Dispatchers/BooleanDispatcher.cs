using GameSaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BooleanDispatcher : Dispatcher<bool>
{
    private static BooleanDispatcher _instance;
    public static BooleanDispatcher Instance => _instance ??= new BooleanDispatcher();
    protected readonly string _uniqueSaveId = "BooleanDispatcherUniqueSaveId";
    public BooleanDispatcher()
    {
        Load();
    }

    [Serializable]
    public class SaveData : IGameData
    {
        public SerializableDictionary<string, bool> listeners;
    }

    public override bool Dispatch(string key)
    {
        if (_listeners.TryGetValue(key, out var value))
            return value;

        Debug.LogWarning($"Boolean with key: '{key}' does not exist. Returning default false.");
        return false;
    }


    public void Load()
    {
        Debug.Log("LOADING DATA");
        GameSaveSystem.GameSaveSystem.Instance.GetData<SaveData>(_uniqueSaveId, (data) =>
        {
            _listeners = data.listeners ?? new SerializableDictionary<string, bool>();
        });
    }

    public void Save()
    {
        Debug.Log("SAVING DATA");
        GameSaveSystem.GameSaveSystem.Instance.SetData(_uniqueSaveId, new SaveData
        {
            listeners = _listeners
        });
    }
}
