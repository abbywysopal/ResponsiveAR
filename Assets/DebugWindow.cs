using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugWindow : MonoBehaviour
{
    [SerializeField]
    TextMeshPro textMesh;

    // Use this for initialization
    void Start()
    {
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogMessage;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogMessage;
    }

    public void LogMessage(string message, string stackTrace, LogType type)
    {
        if(type == LogType.Log) { 
            textMesh.text = message + "\n" + textMesh.text;

        }
    }
}