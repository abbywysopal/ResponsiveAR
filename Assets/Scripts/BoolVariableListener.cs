using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoolVariableListener : MonoBehaviour
{
    [SerializeField] private BoolVariable variable;
    public UnityEvent OnTrue;
    public UnityEvent OnFalse;
    public BoolEvent OnToggle;

    void OnValidate()
    {
        if (variable == null)
        {
            Debug.LogWarning("Field 'variable' not set to a value");
        }
    }

    void OnEnable()
    {
        // if (variable.RuntimeValue) { OnTrue.Invoke(); }
        // else { OnFalse.Invoke(); }

        OnValueChanged(variable.RuntimeValue);
        variable.OnValueChanged += this.OnValueChanged;
    }
    void OnDisable() => variable.OnValueChanged -= this.OnValueChanged;

    void OnValueChanged(bool value)
    {
        OnToggle.Invoke(value);
        if (value) { OnTrue.Invoke(); }
        else { OnFalse.Invoke(); }
    }
}
