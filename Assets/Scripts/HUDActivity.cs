using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HUDActivity : BaseAppActivity
{
    [Header("HUD Activity")]
    public GameObject entityToLaunch;

    protected GameObject cachedEntity;

    void OnDestroy()
    {
        Destroy(cachedEntity);
    }

    public override void StartActivity(ExecutionContext executionContext)
    {
        cachedEntity = GameObject.Instantiate(entityToLaunch, transform);
    }
    public override void StopActivity(ExecutionContext executionContext)
    {
        if (cachedEntity != null) 
        {
            Destroy(cachedEntity);
            cachedEntity = null;
        }
    }
}