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
        
        float x = t1.fontSize * t1.transform.localScale.x;
        float y = t2.fontSize * t2.transform.localScale.x;
        if (x.CompareTo(y) != 0)
        {
            return -1*(x.CompareTo(y));
        }
        else
        {
            return 0;
        }
    }

}

public class VolumeFirst : IComparer<GameObject>
{

    // Compares by FontSize
    public int Compare(GameObject g1, GameObject g2)
    {
        Transform x = g1.transform;
        Transform y = g2.transform;

        float a1 = 1;

        if(x.localScale.x != 0.0f)
        {
            a1 *= x.localScale.x;
        }

        if (x.localScale.y != 0.0f)
        {
            a1 *= x.localScale.y;
        }

        if (x.localScale.z != 0.0f)
        {
            a1 *= x.localScale.z;
        }

        float a2 = 1;

        if (y.localScale.x != 0.0f)
        {
            a2 *= y.localScale.x;
        }

        if (y.localScale.y != 0.0f)
        {
            a2 *= y.localScale.y;
        }

        if (y.localScale.z != 0.0f)
        {
            a2 *= y.localScale.z;
        }


        if(x.localScale.x == 0.0f && x.localScale.y == 0.0f && x.localScale.z == 0.0f)
        {
            a1 = 0.0f;
        }

        if (y.localScale.x == 0.0f && y.localScale.y == 0.0f && y.localScale.z == 0.0f)
        {
            a2 = 0.0f;
        }


        if (a1.CompareTo(a2) != 0)
        {
            return -1 * (a1.CompareTo(a2));
        }
        else
        {
            return 0;
        }

    }

}
