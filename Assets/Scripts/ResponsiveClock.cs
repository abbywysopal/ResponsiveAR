using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ResponsiveClock : MonoBehaviour
{
    [SerializeField]
    GameObject parent;

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
    double LOD1;
    [SerializeField]
    double transitionLOD2;
    [SerializeField]
    double LOD2;
    [SerializeField]
    double LOD3;

    bool LOD1set = true;
    bool LOD2set = false;
    bool LOD3set = false;

    double prevScale;
    double prevDist;
    Vector3 initalPosition;
    Vector3 initalScale;

    float LOD2initTextSize;
    float LOD3initTextSize;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Clock");
        Debug.Log("Scale: " + parent.transform.localScale);

        LOD1parent.SetActive(true);
        LOD2parent.SetActive(false);
        LOD3parent.SetActive(false);
        setTransparency(LOD2text, 10);
        initalPosition = transform.localPosition;
        initalScale = transform.localScale;

        LOD2initTextSize = averageTextSize(LOD2text);
        LOD3initTextSize = averageTextSize(LOD3text);
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


        Debug.Log(parent.name);
        Debug.Log("Scale: " + parent.transform.localScale);
        Debug.Log("Distance local: " + disToHead);

        double scale = parent.transform.localScale.x;
        double scaleDelta = scale - prevDist;
        double distDelta = disToHead - prevDist;

        if(distDelta > .0005f | scaleDelta > .002f)
        {
            Debug.Log("checkIncrease");
            checkIncreaseInLOD(scale, disToHead);
        }

        if(distDelta < -.003f | scaleDelta < -.002f)
        {
            checkDecreaseInLOD(scale, disToHead);
        }

        prevDist = disToHead;
        prevScale = scale;

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
    void checkIncreaseInLOD(double scale, double dist)
    {
        float fontSize2 = averageTextSize(LOD2text);
        Debug.Log("font size: " + fontSize2);
        if((fontSize2 > 12 | dist < .8) & !LOD2set){
            Debug.Log("increaseTrans\n");
            LOD2parent.SetActive(true);
            disableObjects(LOD2objs);
            increaseTransparency(LOD2text);
        }

        // if(scale >= LOD2 & !LOD2set){
        //     setTransparency(LOD2text, 255);
        //     enableObjects(LOD2objs);
        //     LOD2set = true;
        // }


        // if(scale >= LOD3 & !LOD3set){
        //     LOD3parent.SetActive(true);
        //     LOD3set = true;
        // }
    }

    void checkDecreaseInLOD(double scale, double dist)
    {
        // if(size <= transitionLOD2 & !LOD2set){
        //     decreaseTransparency(LOD2text);
        // }

        // if(size <= LOD2 & !LOD2set){
        //     setTransparency(LOD2text, 0);
        //     disableObjects(LOD2objs);
        //     LOD2parent.SetActive(false);
        //     LOD2set = false;
        // }

        // if(size <= LOD3 & !LOD3set){
        //     LOD3parent.SetActive(false);
        //     LOD3set = false;
        // }
    }

    void increaseTransparency(List<TextMeshPro> objs){
        foreach (TextMeshPro obj in objs){
            Color32 color = obj.color;
            byte a = (byte)(color[3] + 10);
            if(color[3] + 10 >= 255){
                a = 255;
            }

            obj.color = new Color32(color[0], color[1], color[2], a);

        }
    }

    void decreaseTransparency(List<TextMeshPro> objs){
        foreach (TextMeshPro obj in objs){
            Color32 color = obj.color;
            byte a = (byte)(color[3] - 10);
            obj.color = new Color32(color[0], color[1], color[2], a);

        }
    }

    void setTransparency(List<TextMeshPro> objs, byte a){
        foreach (TextMeshPro obj in objs){
            Color32 color = obj.color;
            obj.color = new Color32(color[0], color[1], color[2], a);
        }

    }

    float averageTextSize(List<TextMeshPro> objs){
        float totalSize = 0;
        float totalObjs = 0f;
        foreach (TextMeshPro obj in objs){
            totalObjs += 1f;
            totalSize += obj.fontSize * obj.transform.localScale.x;
        }

        return totalSize/totalObjs;

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

    /*
     * 
     * could make hour and minute hand large then decrease size as you scale
     * look at position as well as scale
     * 
     * 
     * 
     */
}
