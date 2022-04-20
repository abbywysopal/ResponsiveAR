using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class NumberDisplay : MonoBehaviour
{
    // Display properties
    public int maxLength;

    // Assign in editor
    [SerializeField] private TextMeshPro textMesh;

    public static UserStudyTask study;

    void OnEnable()
    {
        Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        textMesh.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void setTask(UserStudyTask s)
    {
        study = s;
    }

    public void Enter(string str)
    {
        if (textMesh.text == "Dialing..." || textMesh.text == "INCORRECT")
        {
            Clear();
        }
        Debug.Log("Pressed " + str);
        if (textMesh.text.Length < maxLength)
        {
            textMesh.text += str;
        }
    }

    public void Delete()
    {
        if (textMesh.text == "Dialing..." || textMesh.text == "INCORRECT")
        {
            Clear();
        }
        textMesh.text = textMesh.text.Remove(textMesh.text.Length - 1);
    }

    public void Clear()
    {
        textMesh.text = string.Empty;
    }

    public void Call()
    {
        textMesh.text = "Dialing...";
    }

    public void Enter_keys()
    {
        bool valid = study.touchpad_enter(textMesh.text);
        if (valid)
        {
            textMesh.text = "";
        }
        else
        {
            textMesh.text = "INCORRECT";
        }
    }

}
