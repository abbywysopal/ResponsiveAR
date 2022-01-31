using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Only 1 BaseEntity can exist within a GameObject hierarchy.
public class BaseEntity : MonoBehaviour
{
    protected virtual void Reset() => EnforceHierarchy();
    protected virtual void OnValidate() => EnforceHierarchy();
    private void EnforceHierarchy()
    {
        // BaseEntity[] parentEntities = GetComponentsInParent<BaseEntity>();
        // BaseEntity[] childEntities = GetComponentsInChildren<BaseEntity>();

        // if (parentEntities.Length != 1 || childEntities.Length != 1)
        // {
        //     ARDebug.LogError($"Only one BaseEntity allowed in hierarchy!");
        // }
    }
}
