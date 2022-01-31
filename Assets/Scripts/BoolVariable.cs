using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Bool Variable")]
public class BoolVariable : Variable<bool>
{
    public void ToggleValue()
    {
        SetValue(!RuntimeValue);
        Debug.Log($"BoolVariable {name} set to {RuntimeValue}");
    }
}
