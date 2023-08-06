using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard 
{
    public delegate void OnBlackboardValueChange(string key, object value);
    public event OnBlackboardValueChange onBlackboardValueChange;

    private Dictionary<string, object> blackboardData = new Dictionary<string, object>();

    public void SetOrAddData(string key, object value)
    {
        if (blackboardData.ContainsKey(key))
        {
            blackboardData[key] = value;
        }
        else
        {
            blackboardData.Add(key, value);
        }

        onBlackboardValueChange?.Invoke(key, value);
    }

    public void RemoveBlackboardData(string key)
    {
        blackboardData.Remove(key);
        onBlackboardValueChange?.Invoke(key, null);
    }

    public bool GetBlackboardData<T>(string key, out T value)
    {
        value = default(T);
        if (blackboardData.ContainsKey(key))
        {
            value = (T)blackboardData[key];
            return true;
        }

        return false;
    }

    public bool HasKey(string key) => blackboardData.ContainsKey(key);


}
