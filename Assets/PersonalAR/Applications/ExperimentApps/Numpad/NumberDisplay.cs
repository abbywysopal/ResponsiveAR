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

    public void Enter(string str)
    {
        if(textMesh.text == "Dialing...")
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
        if (textMesh.text == "Dialing...")
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

}
