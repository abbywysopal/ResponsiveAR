using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum ExecutionState { Stopped, Suspended, RunningFull, RunningPartial };

[CreateAssetMenu(menuName = "Applications/AppState")]
public class AppState : ScriptableObject
{
    public static AppState lastAppStarted;

    // Application meta info
    public string appName;
    public string appDesc;
    public Material appLogo;

    // Application save data
    public string appDataFile;
    public AppVariables Variables;

    // Application execution state
    public bool IsInitialized { get; private set; }
    public bool IsRendering { get; private set; }

    public Dictionary<Guid, ActivityType> RunningActivities = new Dictionary<Guid, ActivityType>();
   
    public int NumActivities
    {
        get => RunningActivities.Count;
    }
    public ExecutionState ExecutionState { get; private set; }

    // Application state handlers
    private List<IAppStateListener> listeners = new List<IAppStateListener>();

    void Awake()
    {
        IsInitialized = false;
        IsRendering = false;

        UpdateExecutionState();
    }

    void OnDestroy()
    {
        UpdateExecutionState();
    }

    void OnEnable()
    {
        UpdateExecutionState();
    }

    void OnDisable()
    {
        UpdateExecutionState();
    }

    // ***** VIEW / LISTENER INTERFACE *****
    public void AddListener(IAppStateListener listener)
    {
        listeners.Add(listener);
    }

    public void RemoveListener(IAppStateListener listener)
    {
        listeners.Remove(listener);
    }

    private void AppStart()
    {
        if (IsInitialized == false)
        {
            foreach(var listener in listeners)
            {
                listener.AppStart();
            }
            // OnAppStart.Invoke();
            IsInitialized = true;
        }
    }

    private void AppQuit()
    {
        if (IsInitialized == true)
        {
            foreach(var listener in listeners)
            {
                listener.AppQuit();
            }   
            // OnAppQuit.Invoke();
            IsInitialized = false;
        }
    }

    // ***** CONTROLLER INTERFACE *****
    public void Save(object obj)
    {
        if (!string.IsNullOrEmpty(appDataFile))
        {
            string json = JsonUtility.ToJson(obj, true);
            string filePath = Path.Combine(Application.persistentDataPath, appDataFile);
            using(StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(json);
            }
        }
        else
        {
            Debug.Log("Cannot save file. No file name specified");
        }
    }

    public T Load<T>()
    {
        // If save file not specified, return default object. Can't be saved until file specified.
        if (string.IsNullOrEmpty(appDataFile))
        {
            return default(T);
        }

        // If file specified but doesn't exist, return new default object. File will be created next save.
        string filePath = Path.Combine(Application.persistentDataPath, appDataFile);
        if (!File.Exists(filePath))
        {
            return default(T);
        }

        // If file specified and exists, try to deserialize object from json.
        T obj = default(T);
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string json = reader.ReadToEnd();
                obj = JsonUtility.FromJson<T>(json);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to load object in file {filePath}. Exception {ex}");
        }
        return obj;
    }

    private void SetRenderState(bool value)
    {
        if (IsRendering != value)
        {
            IsRendering = value;
            foreach(var listener in listeners)
            {
                listener.RenderStateChanged(IsRendering);
            }
            // OnRenderStateChanged.Invoke(IsRendering);
        }
    }

    public void ToggleStartOrSuspend()
    {
        // If app has not yet been initialized, call AppStart and start rendering.
        if (IsInitialized == false)
        {
            AppStart();
            SetRenderState(true);
        }
        else
        {
            // If already initialized, toggle the rendering state.
            SetRenderState(!IsRendering);
        }
    }

    //data for app open 
    public Guid StartActivity(ActivityType activityType, ExecutionContext executionContext) 
    {
        // Update internal state
        Guid activityID = System.Guid.NewGuid();
        RunningActivities.Add(activityID, activityType);

        // Create Event Data
        ActivityEventData eventData = new ActivityEventData
        {
            EventTime = System.DateTime.Now,
            ActivityID = activityID,
            ActivityType = activityType,
            StartContext = executionContext
        };
        
        // Debug.Log("START " + appName);

        // Invoke listeners / view updates
        foreach(var listener in listeners)
        {
            listener.OnActivityStart(eventData);
        }

        UpdateExecutionState();

        lastAppStarted = this;
        // If we are currently in immersive mode, we need to close other existing apps after starting this one.

        return activityID;
    }

    //data for app close
    public void StopActivity(Guid activityID, ExecutionContext executionContext)
    {
        if (RunningActivities.ContainsKey(activityID) == false) { return; }

        // Create Event Data
        ActivityEventData eventData = new ActivityEventData
        {
            EventTime = System.DateTime.Now,
            ActivityID = activityID,
            ActivityType = RunningActivities[activityID],
            StopContext = executionContext,
        };

        // Debug.Log("STOP " + appName);

        // Update internal state
        RunningActivities.Remove(activityID);

        // Invoke listeners / view updates
        foreach (var listener in listeners)
        {
            listener.OnActivityStop(eventData);
        }

        UpdateExecutionState();
        
    }

    public void StopAllActivities(ExecutionContext executionContext)
    {
        List<Guid> immutableGuids = RunningActivities.Keys.ToList();
        foreach(Guid activityGuid in immutableGuids)
        {
            StopActivity(activityGuid, executionContext);
        }
    }

    public void StopTutorial()
    {
        ExecutionContext context = new ExecutionContext(new GameObject());
        StopAllActivities(context);
    }

    private void UpdateExecutionState()
    {
        ExecutionState potentialNewState;

        // Determine new potential state
        if (RunningActivities.Count == 0)
        {
            potentialNewState = ExecutionState.Stopped;
        }
        else
        {
            potentialNewState = ExecutionState.RunningFull;
        }

        var ObjectActivities =
            RunningActivities.Where(kv => kv.Value == ActivityType.ObjectMenu);
        if (ObjectActivities.Count() > 0)
        {
            potentialNewState = ExecutionState.RunningPartial;
        }

        // Update state and invoke callbacks if needed
        if (potentialNewState != ExecutionState)
        {
            ExecutionState = potentialNewState;
            listeners.ForEach(handler => handler.OnStateChanged(ExecutionState));
        }
    }

    public void SendMessage(string methodName)
    {
        SendMessage(methodName, null);
    }

    public void SendMessage(string methodName, object value = null, SendMessageOptions options = SendMessageOptions.DontRequireReceiver)
    {
        foreach(var listener in listeners)
        {
            listener.OnMessage(methodName, value, options);
        }
    }
}
