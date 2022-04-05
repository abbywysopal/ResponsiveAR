using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.UI;

public class GetDistance : MonoBehaviour
{
    [SerializeField]
    TextMeshPro dist;
    [SerializeField]
    GameObject parent;

    [SerializeField]
    List<TextMeshPro> fonts;
    [SerializeField]
    List<TextMeshProUGUI> fonts2;


    double distance;
    
    // Start is called before the first frame update
    void Start()
    {
        distance = Math.Abs(calcdist(parent.transform, Camera.main.transform));
        dist.text = "Dis: " + distance.ToString();
        dist.text += "\n Vects: " + parent.transform.position.ToString() + " " + Camera.main.transform.position.ToString();
        dist.text += "\n Scale: " + parent.transform.localScale;

        foreach(TextMeshPro font in fonts)
        {
            font.text = "Size " + getTextSize(font).ToString();
        }

        foreach (TextMeshProUGUI font in fonts2)
        {
            font.text = "Size " + getTextSize(font).ToString();
        }

    }

    // Update is called once per frame
    void Update()
    {
        distance = Math.Abs(calcdist(parent.transform, Camera.main.transform));
        dist.text = "Dis: " + distance.ToString();
        dist.text += "\n Vects: " + parent.transform.position.ToString() + " " + Camera.main.transform.position.ToString();
        dist.text += "\n Scale: " + parent.transform.localScale;

        foreach (TextMeshPro font in fonts)
        {
            font.text = "Size " + getTextSize(font).ToString();
        }

        foreach (TextMeshProUGUI font in fonts2)
        {
            font.text = "Size " + getTextSize(font).ToString();
        }
    }

    double calcdist(Transform t1, Transform t2)
    {
        return (t1.position - t2.position).magnitude;
    }



    public double getTextSize(TextMeshPro t)
    {
        Transform pt = t.transform.parent;
        double scale = t.fontSize * t.transform.localScale.x;
        Debug.Log("t: " + t + ", font: " + scale);
        while (pt != null)
        {
            scale *= pt.transform.localScale.x;
            Debug.Log("pt: " + pt + ", scale: " + pt.transform.localScale.x + ", font: " + scale);
            pt = pt.parent;
        }

        return scale;
    }
    
    public double getTextSize(TextMeshProUGUI t)
    {
        Transform pt = t.transform.parent;
        double scale = t.fontSize * t.transform.localScale.x;
        Debug.Log("t: " + t + ", font: " + scale);
        while (pt != null)
        {
            scale *= pt.transform.localScale.x;
            Debug.Log("pt: " + pt + ", scale: " + pt.transform.localScale.x + ", font: " + scale);
            pt = pt.parent;
        }

        return scale;
    }
}
