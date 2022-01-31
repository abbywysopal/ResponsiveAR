using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveEarth : MonoBehaviour
{
    public GameObject gObj;
    public GameObject Canvas;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        Debug.Log("Scale: " + gObj.transform.localScale);
        Debug.Log("Canvas: " + Canvas.name);
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)){
            if(hit.transform == gObj.transform){
                Debug.Log("HIT");
            // the object identified by hit.transform was clicked
            // do whatever you want
                Debug.Log(hit.transform);
                ScaleTransform(hit.transform);
            }
        }
    }

    void ScaleTransform(Transform transform){
        Vector3 v = new Vector3(.001f, .001f, .001f);
        transform.localScale = transform.localScale + v;
        transform.position = transform.position - 2*v;
        Debug.Log("Scale: " + transform.localScale);
        Debug.Log("Position " + transform.position);

        if(transform.localScale.x > .1f){
            setLOD1(transform);
        }
    }

    void setLOD1(Transform transform){
        Debug.Log("LOD1");
        Canvas.SetActive(true); //enable canvas
    }
}
