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
        Vector3 direction = new Vector3(0.0f, 0.0f, Camera.main.transform.localEulerAngles.y);
        parent.transform.localEulerAngles = direction;
        Debug.Log(direction);

    }
}
