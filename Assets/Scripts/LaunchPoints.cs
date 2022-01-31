using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Utilities.Solvers;

public class LaunchPoints : MonoBehaviour
{
    public enum HUDRange { Near, Far };
    public static LaunchPoints HUDNear;
    public static LaunchPoints HUDFar;

    [Header("HUD Properties")]
    public HUDRange hudRange;

    [Header("Grid Properties")]
    [Range(1, 9)]
    public int gridPoints;
    public int gridRows;
    public float gridRadius;
    public int gridRadialRange;
    public float overlapDelta;

    private List<int> occupancy;
    private string pointsParentName = "LaunchPoints";

    void OnValidate()
    {
        if (gridPoints < 1) { gridPoints = 1; }
        else if (gridPoints > 9) { gridPoints = 9; }
    }

    void Reset()
    {
        gridPoints = 3;
        gridRows = 1;
        gridRadius = 0.5f;
        gridRadialRange = 90;
        overlapDelta = 0.05f;
    }

    void Awake()
    {
        // CreateHUDPoints();
        occupancy = Enumerable.Repeat(0, gridPoints).ToList();

        if (hudRange == HUDRange.Near)
        {
            if (HUDNear != null)
            {
                throw new NotSupportedException("Only one instance of each HUDRange allowed");
            }
            HUDNear = this;
        }
        else if (hudRange == HUDRange.Far)
        {
            if (HUDFar!= null)
            {
                throw new NotSupportedException("Only one instance of each HUDRange allowed");
            }
            HUDFar = this;
        }
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public Transform GetLaunchPoint()
    {
        Transform launchPoints = transform.Find(pointsParentName);

        // Determine next available point including offsets for overlap
        int min = occupancy.Min();
        for(int i = 0; i < occupancy.Count; ++i)
        {
            if (occupancy[i] == min)
            {
                Transform basePoint = launchPoints.GetChild(i);
                Transform offsetPoint = basePoint.GetChild(0);

                // Calculate offsetPoint position        
                Vector3 toCamera = (Camera.main.transform.position - basePoint.transform.position).normalized;
                Vector3 offset = toCamera * overlapDelta * occupancy[i];
                offsetPoint.transform.position += offset;

                // Update occupancy
                occupancy[i]++;
                return offsetPoint;
            }
        }

        // SHOULD NOT HAPPEN.
        return null;
    }

    public void FreeLaunchPoint(Transform point)
    {
        int i = point.GetSiblingIndex();
        occupancy[i] = Mathf.Max(occupancy[i] - 1, 0);
    }

    public void CreateHUDPoints()
    {
        // Delete existing launch points (if any).
        Transform child = transform.Find(pointsParentName);
        if (child != null)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }

        // Create Grid
        GameObject launchPoints = new GameObject(pointsParentName);
        launchPoints.transform.SetParent(transform);

        // Add follow solver
        Follow follow = launchPoints.AddComponent<Follow>();
        follow.DefaultDistance = gridRadius;
        follow.MinDistance = gridRadius - 0.2f;
        follow.MaxDistance = gridRadius + 0.2f;

        // Add grid
        GridObjectCollection grid = launchPoints.AddComponent<GridObjectCollection>();
        grid.SurfaceType = ObjectOrientationSurfaceType.Cylinder;
        grid.Rows = gridRows;
        grid.Radius = gridRadius;
        grid.RadialRange = gridRadialRange;

        // Add base point object
        GameObject basePoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        basePoint.name = "BasePoint";
        basePoint.transform.SetParent(launchPoints.transform);
        basePoint.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        basePoint.GetComponent<MeshRenderer>().enabled = false;

        // Add offset point to base point
        GameObject offsetPoint = GameObject.Instantiate(basePoint);
        offsetPoint.name = "OffsetPoint";
        offsetPoint.transform.SetParent(basePoint.transform);

        for(int i = 1; i < gridPoints; ++i)
        {
            GameObject.Instantiate(basePoint, launchPoints.transform);
        }

        // Update grid positions;
        grid.UpdateCollection();
    }
}
