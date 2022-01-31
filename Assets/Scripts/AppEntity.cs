using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete("Use BaseEntity instead")]
public class AppEntity : MonoBehaviour, IAppEntity
{
    [SerializeField]
    private BasePervasiveApp _parentApp;
    public BasePervasiveApp ParentApp
    {
        get => _parentApp;
        private set => _parentApp = value;
    }

    public void Bind(BasePervasiveApp toBindApp)
    {
        // Unregister from old parent events
        if (ParentApp)
        {
            ParentApp.OnRenderStateChanged -= OnRenderStateChanged;
        }

        // Assign new parent
        ParentApp = toBindApp;

        // Set current render state to match new parent
        OnRenderStateChanged(ParentApp.Rendering);

        // Register to new parent events
        ParentApp.OnRenderStateChanged += OnRenderStateChanged;

    }

    public bool Rendering
    {
        get
        {
            if (_parentApp == null) return false;
            else return _parentApp.Rendering;
        }
    }

    // private bool _rendering;
    // public bool Rendering
    // {
    //     get => _rendering;
    //     private set
    //     {
    //         // If no change, do nothing.
    //         if (_rendering == value) { return; }

    //         // Else, change and handle new value
    //         _rendering = value;
    //         OnRenderStateChanged(_rendering);
    //     }
    // }

    private void OnRenderStateChanged(bool newState)
    {
        int visibleLayer = ParentApp != null ? ParentApp.gameObject.layer :  LayerMask.NameToLayer("Default");
        int newLayer = newState ? visibleLayer : LayerMask.NameToLayer("Ignore Raycast");

        gameObject.SetLayerInChildren(newLayer);
    }

    public static GameObject Instantiate(GameObject template, BasePervasiveApp parentApp)
    {
        GameObject clone = GameObject.Instantiate(template);
        
        AppEntity appEntity;
        if (!clone.TryGetComponent<AppEntity>(out appEntity))
        {
            appEntity = clone.AddComponent<AppEntity>();
        }

        appEntity.Bind(parentApp);
        return clone;
    }

    public static AppEntity Instantiate(AppEntity entity, BasePervasiveApp parentApp)
    {
        GameObject newEntity = AppEntity.Instantiate(entity.gameObject, parentApp);
        return newEntity.GetComponent<AppEntity>();
    }

    void Reset()
    {
        _parentApp = GetComponentInParent<BasePervasiveApp>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_parentApp != null)
        {
            Bind(_parentApp);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
