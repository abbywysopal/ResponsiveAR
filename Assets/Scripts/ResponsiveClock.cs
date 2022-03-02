using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/*
 * TODO:
 *  1. Brainstorm: what varibles can be made continuous?
 *  2. IDEAS: preprocess to group objects into LDO, create alg to find transition val for each LOD
 *  
 *  1. work on start function which determines LOD values based on font size and initial dist
 *  2. increase size of letters in LOD1
 *  
 *  
 * 
 */

public class ResponsiveClock : MonoBehaviour
{
    [SerializeField]
    GameObject parent;

    [SerializeField]
    SphereCollider collider;

    [SerializeField]
    GameObject LOD1parent;
    [SerializeField]
    GameObject LOD2parent;
    [SerializeField]
    List<TextMeshPro> LOD2text = new List<TextMeshPro>();

    [SerializeField]
    List<GameObject> LOD2objs = new List<GameObject>();

    [SerializeField]
    GameObject LOD3parent;

    [SerializeField]
    List<GameObject> LOD3objs = new List<GameObject>();

    [SerializeField]
    List<TextMeshPro> LOD3text = new List<TextMeshPro>();

    [SerializeField]
    double LOD1 = 2;
    [SerializeField]
    double LOD2 = 3.2;
    [SerializeField]
    double LOD3 = 5.2;
    bool setLOD1 = true;
    bool setLOD2 = false;
    bool setLOD3 = false;

    double prevScale;
    double prevDist;
    Vector3 initalPosition;
    Vector3 initalScale;

    double LOD2initTextSize;
    double LOD3initTextSize;

    // Start is called before the first frame update
    void Start()
    {

        LOD1parent.SetActive(true);
        LOD2parent.SetActive(true);
        LOD3parent.SetActive(true);
        setTransparency(LOD2text, 0);
        setTransparency(LOD3text, 0);
        disableObjects(LOD2objs);
        disableObjects(LOD3objs);

        initalPosition = parent.transform.localPosition;
        initalScale = parent.transform.localScale; //smallest form

        LOD2initTextSize = averageTextSize(LOD2text, initalScale.x);
        LOD3initTextSize = averageTextSize(LOD3text, initalScale.x);

    }

    // Update is called once per frame
    void Update()
    {
        /*
         * when increasing transparency, we should check if size has changed
         * need to revert back when decrease in scale
         */
        var headPosition = Camera.main.transform.position;
        double disToHead = calcDist(Camera.main.transform.position, parent.transform.position);
        double scale = parent.transform.localScale.x;
        double ratio = getRatio(Math.Abs(scale), Math.Abs(disToHead));

        continuousFunction(ratio);
        gazeFunction();

    }

    void continuousFunction(double ratio){
        
        if(ratio < LOD1)
        {
            //setLOD1
            setLOD2 = false;
            setLOD3 = false;
            disableObjects(LOD2objs);
            disableObjects(LOD3objs);
            decreaseTransparency(LOD2text);
            decreaseTransparency(LOD3text);
        }
        else if (ratio < LOD2) { 
            //set LOD2
            setLOD2 = true;
            setLOD3 = false;
            enableObjects(LOD2objs);
            disableObjects(LOD3objs);
            decreaseTransparency(LOD3text);
        }
        else if(ratio < LOD3) {
            //set LOD3
            setLOD2 = true;
            setLOD3 = true;
            enableObjects(LOD2objs);
            enableObjects(LOD3objs);
        }
    }

    void gazeFunction(){
        RaycastHit hitInfo;
        if (Physics.Raycast(
                Camera.main.transform.position,
                Camera.main.transform.forward,
                out hitInfo,
                20.0f,
                Physics.DefaultRaycastLayers))
        {
            // If the Raycast has succeeded and hit a hologram
            // hitInfo's point represents the position being gazed at
            // hitInfo's collider GameObject represents the hologram being gazed at
            if(hitInfo.collider == collider){

                Debug.Log("HIT");
                Debug.Log(hitInfo.collider);
                if(setLOD2){
                    increaseTransparency(LOD2text);
                }
                if(setLOD3){
                    increaseTransparency(LOD3text);
                }
            }else{
                Debug.Log("Hit something else");
                Debug.Log(hitInfo.collider);
                if(setLOD2){
                    decreaseTransparency(LOD2text);
                }
                if(setLOD3){
                    decreaseTransparency(LOD3text);
                }
            }
        }else{
            Debug.Log("Out of focus");
            if(setLOD2){
                decreaseTransparency(LOD2text);
            }
            if(setLOD3){
                decreaseTransparency(LOD3text);
            }
        }
    }

    double calcDist(Vector3 obj1, Vector3 obj2)
    {
        double x = obj1.x - obj2.x;
        double y = obj1.y - obj2.y;
        double z = obj1.z - obj2.z;

        double sum = x*x + y*y + z*z;

        return Math.Sqrt(sum);
    }

    //.35m -> 12points
    //.7m 16 points
    //1.5m -> 32 points

    /*
    will change ratio to depend on fontsize
    LOD1	2
    LOD trans	3
    LOD2	3.6
    LOD3 trans	5
    LOD3	6.7
    */

    void checkIncreaseInLOD(double scale, double dist, double ratio)
    {
        double fontSize2 = averageTextSize(LOD2text, scale);
        Debug.Log("font size 2: " + fontSize2);
        double fontSize3 = averageTextSize(LOD3text, scale);
        
        if(ratio < 2)
        {
            Debug.Log("LOD1 set");
        }
        else if (ratio < 3) { 
            //LOD2 transition
            Debug.Log("increaseTrans\n");
            LOD2parent.SetActive(true);
            disableObjects(LOD2objs);
            increaseTransparency(LOD2text);
        }
        else if(ratio < 3.6) {
            //LDO2 set
            increaseTransparency(LOD2text);//call a algo to increase transparency based on gaze alg
            enableObjects(LOD2objs);
           // LOD2set = true;
        }
        else if(ratio < 5){ 
            //LOD3 transition
            setTransparency(LOD2text, 255);
            Debug.Log("increaseTrans\n");
            LOD3parent.SetActive(true);
            disableObjects(LOD3objs);
            increaseTransparency(LOD3text);//call a algo to increase transparency based on gaze alg
        }
        else if (ratio < 6.7) { 
            //LOD3 set
            increaseTransparency(LOD3text);
            enableObjects(LOD3objs);
            //LOD3set = true;
        }
    }

    void checkDecreaseInLOD(double scale, double dist, double ratio)
    {
        double fontSize2 = averageTextSize(LOD2text, scale);
        double fontSize3 = averageTextSize(LOD3text, scale);
        
        if(ratio < 1.5)
        {
            Debug.Log("LOD1 set");
            setTransparency(LOD2text, 0);
        }
        else if (ratio < 2.8) { 
            //LDO2 unset
            Debug.Log("LOD2 unset");
            decreaseTransparency(LOD2text);
            disableObjects(LOD2objs);
            // LOD2set = false;
        }
        else if(ratio < 3.4) {
            //LOD2 transition
            Debug.Log("LOD2 trans dec");
            decreaseTransparency(LOD2text);
            setTransparency(LOD3text, 0);
            LOD3parent.SetActive(false);
        }
        else if(ratio < 4.8){ 
            //LOD3 unset
            decreaseTransparency(LOD3text);
            disableObjects(LOD3objs);
            // LOD3set = false;
        }
        else if (ratio < 6.5) { 
            //LOD3 transition
            decreaseTransparency(LOD3text);
        }
    }

    void increaseTransparency(List<TextMeshPro> objs){
        if(objs[0].color[3] < 255) { 
            foreach (TextMeshPro obj in objs){
                Color32 color = obj.color;
                byte a = (byte)(color[3] + 1);
                if(color[3] + 1 >= 255){
                    a = 255;
                }
                obj.color = new Color32(color[0], color[1], color[2], a);
            }
        }
    }

    void decreaseTransparency(List<TextMeshPro> objs){
        if(objs[0].color[3] > 0) { 
            foreach (TextMeshPro obj in objs){
                Color32 color = obj.color;
                byte a = (byte)(color[3] - 1);
                if(color[3] - 1 <= 0){
                    a = 0;
                }
                obj.color = new Color32(color[0], color[1], color[2], a);
            }
        }
    }

    void setTransparency(List<TextMeshPro> objs, byte a){
        foreach (TextMeshPro obj in objs){
            Color32 color = obj.color;
            obj.color = new Color32(color[0], color[1], color[2], a);
        }

    }

    double averageTextSize(List<TextMeshPro> objs, double scale){
        double totalSize = 0;
        double totalObjs = 0f;
        foreach (TextMeshPro obj in objs){
            totalObjs += 1f;
            totalSize += obj.fontSize * scale;
        }

        return totalSize/totalObjs;

    }

    double textSizeTMP(TextMeshPro obj){
        return  obj.fontSize * obj.transform.localScale.x;
    }


    void disableObjects(List<GameObject> objs)
    {
        foreach (GameObject obj in objs){
            obj.SetActive(false);
        }
    }

    void enableObjects(List<GameObject> objs)
    {
        foreach (GameObject obj in objs){
            obj.SetActive(true);
        }
    }

    //function to determin scale to dist ratio
    double getRatio(double scale, double dist){
        return ((1000*scale)/(5*dist));
    }

    /*
     * 
     * could make hour and minute hand large then decrease size as you scale
     * look at position as well as scale
     * 
     * 
     * 
     */
}
