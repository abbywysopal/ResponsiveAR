using System;
using UnityEngine;

public class ExecutionContext
{
    public GameObject SourceObject;

    private ExecutionContext() {}
    public ExecutionContext(GameObject sourceObject)
    {
        SourceObject = sourceObject;
    }
}

public class ActivityEventData
{
    public DateTime EventTime;
    public Guid ActivityID;
    public ActivityType ActivityType;

    // Optional Values
    public ExecutionContext StartContext;
    public ExecutionContext StopContext;
}
