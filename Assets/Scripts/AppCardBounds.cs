using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.UI;
using Microsoft.MixedReality.Toolkit.Utilities;

public class AppCardBounds : MonoBehaviour
{
    [SerializeField] private GridObjectCollection _appContentParent;

    private bool _cardInBounds;
    private bool CardInBounds
    {
        get
        {
            return _cardInBounds;
        }
        set
        {
            _cardInBounds = value;
            GetComponent<MeshRenderer>().enabled = _cardInBounds;
        }
    }

    private GameObject _targetPinnable;

    private Collider _targetPinnableBounds;
    private Vector3 _pointerPosition;

    // Start is called before the first frame update
    void Start()
    {
        CardInBounds = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Pinnable")
        {
            return;
        }

        Bounds bounds = GetComponent<Collider>().bounds;
        if (bounds.Contains(_pointerPosition))
        {
            CardInBounds = true;
            _targetPinnable = other.transform.parent.transform.parent.gameObject;
            _targetPinnableBounds = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        CardInBounds = false;
    }

    public void OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        if (eventData.Pointer.BaseCursor != null)
        {
            _pointerPosition = eventData.Pointer.BaseCursor.Position;
        }
    }

    public void OnPointerUp(MixedRealityPointerEventData eventData)
    {
    }

    // void OnPointerDown(MixedRealityPointerEventData eventData)
    // {
    //     _pointerDown = true;
    //     _pointerPosition = eventData.Pointer.Position;
    //     ARDebug.Log($"Pointer down position:")
    // }

    // void OnPointerUp(MixedRealityPointerEventData eventData)
    // {
    //     _pointerDown = false;
    // }
}
