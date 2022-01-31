using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Applications/AppList")]
public class AppList : ScriptableObject
{
    public List<AppState> appList;
}
