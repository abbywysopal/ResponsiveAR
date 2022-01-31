using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;

public class AppCard : BaseEntity
{
    public AppState AppState;

    [SerializeField] private TMPro.TextMeshPro _headerText;
    // [SerializeField] private MeshFilter _headerBackPlate;

    [SerializeField] private Collider _pinnableBounds;
    [SerializeField] private ObjectManipulator _objectManipulator;

    void OnValidate()
    {
        if (_headerText != null && AppState != null)
        {
            _headerText.text = AppState.appName;
        }
    }

    void OnEnable(){}

    void OnDisable(){}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        Debug.Log($"Pointer: {eventData.Pointer.PointerName}, Count: {eventData.Count}");
    }
}
