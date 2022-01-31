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

    // Assign at runtime
    private System.Guid activityID;
    [ReadOnly] public string activityIDDebug;

    void OnEnable()
    {
        Clear();
    }

    // Start is called before the first frame update
    void Start()
    {
        textMesh.color = Color.white;
        activityID = GetComponentInParent<BaseAppActivity>().activityID;
        activityIDDebug = activityID.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enter(string str)
    {
        if (textMesh.text.Length < maxLength)
        {
            textMesh.text += str;
        }
        textMesh.color = Color.white;
    }

    public void Delete()
    {
        textMesh.text = textMesh.text.Remove(textMesh.text.Length - 1);
        textMesh.color = Color.white;
    }

    public void Clear()
    {
        textMesh.text = string.Empty;
        textMesh.color = Color.white;
    }

    public void Validate()
    {
        int entered = System.Int32.Parse(textMesh.text);
    }

    public void CloseActivity()
    {
        var ec = new ExecutionContext(this.gameObject);
        GetComponentInParent<BaseAppActivity>().appState.StopActivity(activityID, ec);   
    }
}
