using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveCoffee : MonoBehaviour
{

    public GameObject coffeeObj;
    public Text t;

    // Start is called before the first frame update
    void Start()
    {
        t.text = "COFFEE";
    }

    // Update is called once per frame
    void Update()
    {
        
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
        t.text = "COFFEE\nSLEEP\nREPEAT";

        // Calculate *screen* position (note, not a canvas/recttransform position)
        // Vector2 canvasPos;
        // Vector3 screenPoint = Camera.main.WorldToScreenPoint(coffeeObj.transform.position);


        // text.text = "COFFEE";
        
        // Convert screen position to Canvas / RectTransform space <- leave camera null if Screen Space Overlay
        //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, Camera.main, out canvasPos);
        
        // Set
        // markerRtra.localPosition = canvasPos;
    }

}
