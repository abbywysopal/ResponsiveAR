using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Utilities;

public static class PervasiveAppRegistry
{
    private static Dictionary<string, BasePervasiveApp> registry = new Dictionary<string, BasePervasiveApp>();

    public static IList<BasePervasiveApp> GetAllApps()
    {
        return registry.Values.ToList();
    }

    public static bool AddApp<T>(T appInstance) where T : BasePervasiveApp
    {
        if (appInstance == null || string.IsNullOrEmpty(appInstance.appId) ||
            string.IsNullOrEmpty(appInstance.appState.appName))
        {
            // Adding a null instance is not supported.
            return false;
        }

        // T existingApp;
        // if (TryGetApp<T>(out existingApp, appInstance.appInfo.name))
        // {
        //     // App already exists
        //     return false;
        // }

        if (!registry.ContainsKey(appInstance.appId))
        {
            // Can't add duplicate app guid
            registry.Add(appInstance.appId, appInstance);
            return true;
        }

        return false;
    }

    // Try to get app based on the name. Returns first instance of app with that name.
    // if name is null or empty, app returns first instance of app type T.
    public static bool TryGetApp<T>(out T outInstance, string name = null) where T : BasePervasiveApp
    {
        outInstance = null;

        if (registry.ContainsKey(name))
        {
            outInstance = (T)registry[name];
            return true;
        }

        if (string.IsNullOrEmpty(name))
        {
            foreach(var app in registry.Values)
            {
                if (typeof(T).IsAssignableFrom(app.GetType()))
                {
                    outInstance = (T)app;
                    return true;
                }
            }
        }

        return false;
    }    
}

[System.Serializable]
public struct PervasiveAppInfo
{
    // public int id;
    public string name;
    public string description;
    public Material logo;
}

public class BasePervasiveApp : MonoBehaviour, IAppStateListener
{
    [Header("Pervasive App Settings")]
    public AppState appState;
    public string appId
    {
        get => appState.GetInstanceID().ToString();
    }

    public bool Rendering
    {
        get => appState.IsRendering;
    }

    public System.Action<bool> OnRenderStateChanged;

    void Reset()
    {
        // Test that Content layer is there. If not, create one.
        if (transform.Find("ContentRoot") == null)
        {
            GameObject empty = new GameObject("ContentRoot");
            empty.transform.parent = this.transform;
        }
    }

    protected virtual void Awake()
    {
        // Add new app to app registry.
        PervasiveAppRegistry.AddApp(this);
    }

    void OnEnable()
    {
        appState.AddListener(this);
    }

    void OnDisable()
    {
        appState.RemoveListener(this);
    }

    public virtual void AppStart() {}
    public virtual void AppQuit() {}

    public virtual void RenderStateChanged(bool toValue)
    {
        OnRenderStateChanged?.Invoke(toValue);
    }

    public void ToggleStartOrSuspend()
    {
        appState.ToggleStartOrSuspend();
    }

    public void OnActivityStart(ActivityEventData eventData) {}
    public void OnActivityStop(ActivityEventData eventData) {}
    public void OnStateChanged(ExecutionState executionState) {}

    public void OnMessage(string methodName, object value, SendMessageOptions options)
    {
        this.gameObject.SendMessage(methodName, value, options);
    }
}
