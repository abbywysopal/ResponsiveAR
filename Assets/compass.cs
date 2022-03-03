using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass : MonoBehaviour
{

    [SerializeField]
    GameObject parent;
   
    //compass not changing 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        var v = Camera.main.transform.forward;
 
        parent.transform.localEulerAngles = Camera.main.transform.forward;

        Debug.Log("forward: " +  v);

    }
}
