using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppCardsController : MonoBehaviour
{
    private bool IsOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Open()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.GetComponent<ScaleTween>().TweenIn();
        }

        IsOpen = true;
    }

    void Close()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.GetComponent<ScaleTween>().TweenOut();
        }

        IsOpen = false;
    }

    public void Toggle()
    {
        if (IsOpen) { Close(); }
        else { Open(); }
    }

    void AddAppCard(AppCard card)
    {
        
    }
}