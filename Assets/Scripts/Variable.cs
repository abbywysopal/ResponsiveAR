using System;
using UnityEngine;

[Serializable]
public class Variable<T> : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField]
    private T InitialValue;
    [System.NonSerialized]
    public T RuntimeValue;
    public Action<T> OnValueChanged;

    public void OnAfterDeserialize()
    {
        RuntimeValue = InitialValue;
    }

    public void OnBeforeSerialize() {}

    public void SetValue(T newValue)
    {
        if (!RuntimeValue.Equals(newValue))
        {
            RuntimeValue = newValue;
            OnValueChanged?.Invoke(RuntimeValue);
        }
    }

    public T GetValue()
    {
        return RuntimeValue;
    }
}
