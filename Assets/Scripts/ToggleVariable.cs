using System;
using UnityEngine;

public class ToggleVariable : MonoBehaviour
{
    bool value = false;
    [SerializeField]
    GameObject table;
    public void ToggleValue()
    {
        Debug.Log("new value: " + !value);
        table.transform.position =  Camera.main.transform.position + new Vector3(0.0f, 0.1f, 1.0f);
        value = !value;
        table.SetActive(value);
    }

    public void ToggleOff()
    {
        table.transform.position =  Camera.main.transform.position + new Vector3(0.0f, 0.1f, 1.0f);
        value = false;
        table.SetActive(false);
    }

}
