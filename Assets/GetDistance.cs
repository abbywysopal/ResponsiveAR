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

    double distance;
    
    // Start is called before the first frame update
    void Start()
    {
        distance = Math.Abs(calcdist(parent.transform, Camera.main.transform));
        dist.text = "Dis: " + distance.ToString();
        dist.text += "\n Vects: " + parent.transform.position.ToString() + " " + Camera.main.transform.position.ToString();


    }

    // Update is called once per frame
    void Update()
    {
        distance = Math.Abs(calcdist(parent.transform, Camera.main.transform));
        dist.text = "Dis: " + distance.ToString();
        dist.text += "\n Vects: " + parent.transform.position.ToString() + " " + Camera.main.transform.position.ToString();

    }
    double calcdist(Transform t1, Transform t2)
    {
        return (t1.position - t2.position).magnitude;
    }

}
