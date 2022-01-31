using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AppStateListener : MonoBehaviour, IAppStateListener
{
    [SerializeField] private AppState appState;

    // [Tooltip("Send AppState event messages to other components.")]
    // [SerializeField] private bool RouteAppEvents;

    public UnityEvent OnAppStart;
    public UnityEvent OnAppRenderOn;
    public UnityEvent OnAppRenderOff;
    public UnityEvent OnAppQuit;

    // Activity Events
    public UnityEvent OnMainActivityStart;
    public UnityEvent OnMainActivityStop;
    public UnityEvent OnObjectActivityStart;
    public UnityEvent OnObjectActivityStop;

    // Execution State Events
    // public UnityEvent OnExecutionStateChanged;
    public UnityEvent OnExecutionStopped;
    public UnityEvent OnExecutionSuspended;
    public UnityEvent OnExecutionRunningFull;
    public UnityEvent OnExecutionRunningPartial;
 
    void OnEnable()
    {
        // Match current app rendering state
        if (appState != null)
        {
            // RenderStateChanged(appState.IsRendering);
            OnStateChanged(appState.ExecutionState);
        }
        appState?.AddListener(this);
    }

    void OnDisable()
    {
        appState?.RemoveListener(this);
    }

    public void SetAppState(AppState newAppState)
    {
        if (this.appState != newAppState)
        {
            OnDisable();
            this.appState = newAppState;
            OnEnable();
        }
    }

    public AppState GetAppState()
    {
        return this.appState;
    }

    public void AppStart()
    {
        OnAppStart.Invoke();

        // if (RouteAppEvents)
        //     gameObject.SendMessage("AppStart", SendMessageOptions.DontRequireReceiver);
    }

    public void AppQuit()
    {
        OnAppQuit.Invoke();

        // if (RouteAppEvents)
        //     gameObject.SendMessage("AppQuit", SendMessageOptions.DontRequireReceiver);
    }

    public void RenderStateChanged(bool newValue)
    {
        if (newValue == true)
        {
            // Rendering turned on
            OnAppRenderOn.Invoke();
        }
        else
        {
            // Rendering turned off
            OnAppRenderOff.Invoke();
        }

        // if (RouteAppEvents)
        //     gameObject.SendMessage("RenderStateChanged", newValue, SendMessageOptions.DontRequireReceiver);
    }

    public void OnStateChanged(ExecutionState newExecutionState)
    {
        if (newExecutionState == ExecutionState.Stopped)
        {
            OnExecutionStopped.Invoke();
        }
        else if (newExecutionState == ExecutionState.Suspended)
        {
            OnExecutionSuspended.Invoke();
        }
        else if (newExecutionState == ExecutionState.RunningFull)
        {
            OnExecutionRunningFull.Invoke();
        }
        else if (newExecutionState == ExecutionState.RunningPartial)
        {
            OnExecutionRunningPartial.Invoke();
        }
    }

    public void OnActivityStart(ActivityEventData eventData)
    {
        if (eventData.ActivityType == ActivityType.MainMenu)
        {
            OnMainActivityStart.Invoke();
        }
        else if (eventData.ActivityType == ActivityType.ObjectMenu)
        {
            OnObjectActivityStart.Invoke();
        }
    }

    public void OnActivityStop(ActivityEventData eventData)
    {
        if (eventData.ActivityType == ActivityType.MainMenu)
        {
            OnMainActivityStop.Invoke();
        }
        else if (eventData.ActivityType == ActivityType.ObjectMenu)
        {
            OnObjectActivityStop.Invoke();
        }
    }

    public void OnMessage(string methodName, object value, SendMessageOptions options)
    {
        this.gameObject.SendMessage(methodName, value, options);
    }
}
