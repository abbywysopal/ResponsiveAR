using System;
using UnityEngine;

public class BoolVariable : MonoBehaviour
{
    [SerializeField]
    bool value;
    [SerializeField]
    GameObject table;
    public void ToggleValue()
    {
        value = !value;
        table.SetActive(value);
    }
}
