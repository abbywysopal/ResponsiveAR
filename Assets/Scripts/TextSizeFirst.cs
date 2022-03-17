using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class TextSizeFirst : IComparer<TextMeshPro>
{
    
    // Compares by FontSize
    public int Compare(TextMeshPro t1, TextMeshPro t2)
    {
        
        double x = getTextSize(t1);
        double y = getTextSize(t2);
        if (x.CompareTo(y) != 0)
        {
            return -1*(x.CompareTo(y));
        }
        else
        {
            return 0;
        }
    }

    public double getTextSize(TextMeshPro t)
    {

        Transform pt = t.transform.parent;
        double scale = t.fontSize * t.transform.localScale.x;
        while (pt != null)
        {
            scale *= pt.transform.localScale.x;
            pt = pt.parent;
        }

        return scale;
    }

}

public class VolumeFirst : IComparer<GameObject>
{

    // Compares by FontSize
    public int Compare(GameObject g1, GameObject g2)
    {
        double x = getObjectSize(g1);

        double y = getObjectSize(g2);

        if (x.CompareTo(y) != 0)
        {
            return -1 * (x.CompareTo(y));
        }
        else
        {
            return 0;
        }

    }

    double getObjectSize(GameObject g)
    {
        Transform t = g.transform;
        Transform pt = t.transform.parent;
        double volume = t.transform.localScale.x * t.transform.localScale.y * t.transform.localScale.z;
        double scale = volume;
        while (pt != null)
        {
            volume = pt.transform.localScale.x * pt.transform.localScale.y * pt.transform.localScale.z;
            scale *= volume;
            pt = pt.parent;
        }

        return scale;
    }

}
