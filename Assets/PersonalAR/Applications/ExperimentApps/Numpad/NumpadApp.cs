using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumpadApp : BasePervasiveApp
{
    [SerializeField] private GameObject numpadPrefab;
    // [SerializeField] private HUDManager launchPoints;

    private GameObject numpad;

    // Start is called before the first frame update
    void Start()
    {
        numpad = GameObject.Instantiate(numpadPrefab);
        numpad.transform.position = new Vector3(10, 10, 10);
        numpad.GetComponent<ScaleTween>().TweenOut();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void AppStart()
    {
        base.AppStart();
    }

    public override void AppQuit()
    {
        base.AppQuit();
    }

    public override void RenderStateChanged(bool toValue)
    {
        base.RenderStateChanged(toValue);

        if (toValue == true)
        {
            Transform launchPoint = LaunchPoints.HUDNear.GetLaunchPoint();
            numpad.transform.SetPositionAndRotation(launchPoint.position, launchPoint.rotation);
            numpad.GetComponent<ScaleTween>().TweenIn();
        }
        else
        {
            numpad.GetComponent<ScaleTween>().TweenOut();
        }
    }
}
