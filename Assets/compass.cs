using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class compass : MonoBehaviour
{

    [SerializeField]
    GameObject parent;
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        
        var v = Camera.main.transform.forward;
 
        //parent.transform.rotation = (0,0,0);

        Debug.Log("forward: " +  v);



    }
}
