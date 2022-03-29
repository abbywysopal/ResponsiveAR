using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    int min;
    int max;
    
    // Start is called before the first frame update
    void Start()
    {
        min = 54;
        max = 78;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float slider_val()
    {
        return ((float)min/max);
    }
}
