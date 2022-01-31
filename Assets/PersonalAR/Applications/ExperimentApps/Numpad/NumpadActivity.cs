using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumpadActivity : HUDActivity
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartActivity(ExecutionContext executionContext)
    {
        base.StartActivity(executionContext);

        // Ready startup animation
        cachedEntity.GetComponent<ScaleTween>().tweenOutOnStart = false;
        cachedEntity.GetComponent<ScaleTween>().TweenIn();

        // Initialize with random codes
    }
}
