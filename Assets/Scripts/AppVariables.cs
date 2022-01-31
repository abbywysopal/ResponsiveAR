using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class AppVariables : ScriptableObject, ISavable
{
    public string FileName;
    [ReadOnly]
    public string FilePath;

    void OnValidate()
    {
        if (!String.IsNullOrEmpty(FileName))
        {
            FilePath = Path.Combine(Application.persistentDataPath, FileName);
        }
        else
        {
            FilePath = string.Empty;
        }
    }
    void OnEnable()
    {
        if (!String.IsNullOrEmpty(FileName))
        {
            FilePath = Path.Combine(Application.persistentDataPath, FileName);
        }
        else
        {
            FilePath = string.Empty;
        }
    }

    public bool Save()
    {
        try
        {
            using(StreamWriter writer = new StreamWriter(FilePath))
            {
                string json = JsonUtility.ToJson(this, true);
                writer.Write(json);
            }
            return true;
        }
        catch(Exception ex)
        {
            Debug.LogException(ex, this);
            return false;
        }
    }

    public bool Load()
    {
        try
        {
            using(StreamReader reader = new StreamReader(FilePath))
            {
                string json = reader.ReadToEnd();
                JsonUtility.FromJsonOverwrite(json, this);
            }
            return true;
        }
        catch(Exception ex)
        {
            Debug.LogException(ex, this);
            return false;
        }
    }
}
