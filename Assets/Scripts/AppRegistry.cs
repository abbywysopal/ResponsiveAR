using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class AppRegistry
{
    private static Dictionary<string, BaseApp> registry = new Dictionary<string, BaseApp>();

    public static IList<BaseApp> GetAllApps()
    {
        return registry.Values.ToList();
    }

    public static bool AddApp<T>(T appInstance) where T : BaseApp
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
    public static bool TryGetApp<T>(out T outInstance, string name = null) where T : BaseApp
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



