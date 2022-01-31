using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveObject : MonoBehaviour
{
    public GameObject gObj; //game object you want to add reponsive behavior to
    public List<GameObject> LOD1obj;

    public float LOD1;
    
    //create multiple canvases that will be enabled and disabled depending on the level of item/size of canvas

    //need to find a way to transition between disable and enabled better
    void Start()
    {
        Debug.Log("Start: " + gObj.name);
        Debug.Log("Scale: " + gObj.transform.localScale);

        foreach (GameObject obj in LOD1obj){
            Debug.Log("obj: " + obj.name);
            obj.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // if (Physics.Raycast(ray, out hit)){
        // // the object identified by hit.transform was clicked
        // // do whatever you want
        //     Debug.Log("HIT: " + hit.transform);
        //     ScaleTransform(hit.transform);
        // }
    }

    void ScaleTransform(Transform transform){
        Debug.Log("Scale: " + transform);
        Vector3 v = new Vector3(.001f, .001f, .001f);
        transform.localScale = transform.localScale + v;
        transform.position = transform.position - 2*v;

        if(transform.localScale.x > LOD1){
            
        }

        //for next level of detail disable canvas 1 and enable canvas 2
    }

    void enableLOD1(GameObject c){
        Debug.Log("Canvas: "+ c.name);
        c.SetActive(true);
    }


}
