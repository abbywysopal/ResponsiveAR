
using System;
using System.ComponentModel;

public interface ISavable
{
    bool Save();
    bool Load();
}

// public class JSONSavable<T> : ISavable
// {
//     public T deserialized { get; private set; }

//     public void Save(string file)
//     {
//         using(StreamWriter writer = new StreamWriter(file))
//         {
//             string json = JsonUtility.ToJson(this);
//             writer.Write(json);
//         } 
//     }

//     public void Load(string file)
//     {
//         using(StreamReader reader = new StreamReader(file))
//         {
//             string json = reader.ReadToEnd();
//             deserialized = JsonUtility.FromJson<T>(json);
//         }
//     }
// }