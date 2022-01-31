using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleTween : MonoBehaviour
{
    // Animation parameters
    public LeanTweenType inTween;
    public Vector3 inTweenTo;
    public LeanTweenType outTween;
    public Vector3 outTweenTo;
    public bool tweenOutOnStart;
    public float duration;
    public float delay;

    // Events
    public UnityEvent OnTweenInComplete;
    public UnityEvent OnTweenOutComplete;

    // Is true only if most recent tween call was TweenIn.
    public bool IsTweenedIn { get; private set; }

    public void TweenIn()
    {
        // transform.localScale = Vector3.zero;
        // LeanTween.scale(gameObject, inTweenTo, duration)
        //     .setDelay(delay)
        //     .setEase(inTween)
        //     .setOnComplete(delegate () { OnTweenInComplete.Invoke(); });
        // IsTweenedIn = true;
    
        TweenIn(null);
    }

    public void TweenIn(Action onCompleteCallback)
    {
        transform.localScale = Vector3.zero;
        TweenTo(inTween, inTweenTo, delegate ()
        {
            onCompleteCallback?.Invoke();
            OnTweenInComplete.Invoke();

        });
        IsTweenedIn = true; 
    }

    public void TweenOut()
    {
        // LeanTween.scale(gameObject, outTweenTo, duration)
        //     .setDelay(delay)
        //     .setEase(outTween)
        //     .setOnComplete(delegate () { OnTweenOutComplete.Invoke(); });
        // IsTweenedIn = false;
        TweenOut(null);
    }

    public void TweenOut(Action onCompleteCallback)
    {
        TweenTo(outTween, outTweenTo, delegate ()
        {
            onCompleteCallback?.Invoke();
            OnTweenOutComplete.Invoke();
        });
        IsTweenedIn = false;
    }

    public void TweenTo(LeanTweenType tweenType, Vector3 tweenTo, Action callback)
    {
        LeanTween.scale(gameObject, tweenTo, duration)
            .setDelay(delay)
            .setEase(tweenType)
            .setOnComplete(callback);   
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (tweenOutOnStart)
        {
            TweenOut();
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }
}
