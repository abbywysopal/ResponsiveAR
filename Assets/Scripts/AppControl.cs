using System;
using UnityEngine;

public class AppControl : MonoBehaviour
{
    public AppState appState;
    public ActivityType activityToLaunch;

    private Guid cachedGuid;

    void OnEnable()
    {
        cachedGuid = Guid.Empty;
    }

    public void StartActivity()
    {
    }

    public void StopActivity()
    {
    }

    public void ToggleActivity()
    {
        if (cachedGuid.Equals(Guid.Empty))
        {
            StartActivity();
        }
        else
        {
            StopActivity();
        }
    }
}