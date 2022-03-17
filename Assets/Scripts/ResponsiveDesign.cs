using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.UI;

/*
 * ASSUMPTIONS:
 *  1. Parent is in middle of object
 *  2. collider is in middle of object
 * 
 */

/*
 * TODO:
 *  1. Brainstorm: what varibles can be made continuous?

 *  2. increase size of letters in LOD1
 * 
 */

public class ResponsiveDesign : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    Collider collider;

    double objectMedian;
    double highestRatio = (double).0;


    IDictionary<int, LOD_TMP> text = new Dictionary<int, LOD_TMP>();
    IDictionary<int, LOD_Obj> objects = new Dictionary<int, LOD_Obj>();
    IDictionary<int, LOD_Interact> interaction = new Dictionary<int, LOD_Interact>();

    // Start is called before the first frame update
    void Start()
    {

        SetUp();
        var headPosition = Camera.main.transform.position;
        double disToHead = calcDist(Camera.main.transform.position, parent.transform.position);
        double scale = parent.transform.localScale.x;
        double ratio = getRatio(Math.Abs(scale), Math.Abs(disToHead));
        setUpLOD(ratio, scale, disToHead);

        continuousFunction(ratio);
        gazeFunction();

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


    void SetUp()
    {
        TextMeshPro[] allTMP;
        Interactable[] interactables;
        Transform[] allTransforms;

        allTMP = parent.GetComponentsInChildren<TextMeshPro>();
        interactables = parent.GetComponentsInChildren<Interactable>();
        allTransforms = parent.GetComponentsInChildren<Transform>();


        List<GameObject> allObjects = new List<GameObject>();
        List<TextMeshPro> allText = new List<TextMeshPro>(allTMP);
        List<Interactable> allInteraction = new List<Interactable>(interactables);

        allTransforms = parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in allTransforms)
        {
            if (child.childCount == 0)
            {
                TextMeshPro tmp = child.GetComponent<TextMeshPro>();
                Interactable i = child.GetComponent<Interactable>();
                if (tmp == null && i == null)
                {
                    allObjects.Add(child.gameObject);
                }

                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }

        Debug.Log("allTransforms");
        Debug.Log(allTransforms.Length);
        Debug.Log("allObjects");
        Debug.Log(allObjects.Count);
        Debug.Log("allText");
        Debug.Log(allText.Count);
        Debug.Log("allInteraction");
        Debug.Log(allInteraction.Count);

        if (allText.Count > 0)
        {
            getTextSize(allText[0]);

            GroupTMP(allText);
        }

        if (allObjects.Count > 0)
        {
            GroupObjects(allObjects);
        }

        if (allInteraction.Count > 0)
        {
            GroupInteraction(allInteraction);
            foreach(KeyValuePair<int, LOD_Interact> kvp in interaction)
            {
                Debug.Log("LOD" + kvp.Key + ":");
                List<Interactable> list = kvp.Value.getInteractables();
                foreach(Interactable t in list)
                {
                    Debug.Log(t);
                }
            }
        }

    }

    void setUpLOD(double r, double s, double d)
    {
        setUpLODText(r,s,d);
        setUpLODObjects(r,s,d);
        setUpLODInteraction(r,s,d);
    }


    //1 point = .0003527 m
    //12 points = .004233
    void setUpLODText(double r, double s, double d)
    {

        for(int i = 1; i <= text.Count; i++)
        {
            double textSize = text[i].getTextSize();
            double readable = (double).11;
            double size = readable / textSize;
            double result = size * r;
            if(i == 1) { result = 0; }
            text[i].setRatio(result);
            if(result > highestRatio) { highestRatio = result; }
            Debug.Log("LOD: " + i.ToString());
            Debug.Log("textSize: " + textSize.ToString());
            Debug.Log("size: " + size.ToString());
            Debug.Log("result: " + result.ToString());
            Debug.Log("ratio: " + r.ToString());
        }

    }

    void setUpLODObjects(double r, double s, double d)
    {
        //TODO: FIX THIS FUNCTION
/*        objectMedian*/
        Transform t = parent.transform;
        double largestObj = objects[0].getLocalSize();
        objects[0].setRatio(0);

        Debug.Log("objectMedian: " + objectMedian.ToString());

        for(int i = 1; i < objects.Count; i++)
        {
            double sizeObj = objects[i].getLocalSize();
            double result = r * (objectMedian / sizeObj);
            objects[i].setRatio(result);
            Debug.Log("LOD: " + i.ToString());
            Debug.Log("sizeObj: " + sizeObj.ToString());
            Debug.Log("result: " + result.ToString());
            Debug.Log("ratio: " + r.ToString());
        }
    }

    void setUpLODInteraction(double r, double s, double d)
    {
        foreach(KeyValuePair<int, LOD_Interact> kvp in interaction)
        {
            Debug.Log(highestRatio);
            kvp.Value.setRatio(highestRatio);
        }

    }

/*    void setLOD(int i, bool val)
    {
        if (text.ContainsKey(i) && val)
        {
            text[i].setLOD(val);
        }
        if (objects.ContainsKey(i))
        {
            objects[i].setLOD(val);
        }
        if (interaction.ContainsKey(i))
        {
            interaction[i].setLOD(val);
        }
    }*/

    void continuousFunction(double ratio)
    {

        foreach(KeyValuePair<int, LOD_TMP> kvp in text)
        {
            double r = kvp.Value.getRatio();
            if (ratio < r)
            {
                kvp.Value.setSet(false);
                kvp.Value.decreaseTransparency();
            }
            else
            {
                kvp.Value.setLOD(true);
                kvp.Value.increaseTransparency();
            }
        }
        foreach (KeyValuePair<int, LOD_Obj> kvp in objects)
        {
            double r = kvp.Value.getRatio();
            if (ratio < r)
            {
                kvp.Value.setLOD(false);
            }
            else
            {
                kvp.Value.setLOD(true);
            }
        }
        foreach (KeyValuePair<int, LOD_Interact> kvp in interaction)
        {
            double r = kvp.Value.getRatio();
            if (ratio < r)
            {
                kvp.Value.setLOD(false);
            }
            else
            {
                kvp.Value.setLOD(true);
            }
        }
    }

    void gazeFunction()
    {
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
            if (hitInfo.collider == collider)
            {

                //Debug.Log("HIT");
                //Debug.Log(hitInfo.collider);
                foreach (KeyValuePair<int, LOD_TMP> kvp in text)
                {
                    bool set = kvp.Value.getSet();
                    if (set)
                    {
                        text[kvp.Key].increaseTransparency();
                    }

                }
            }
            else
            {
                //Debug.Log("Hit something else");
                //Debug.Log(hitInfo.collider);
                foreach (KeyValuePair<int, LOD_TMP> kvp in text)
                {
                    bool set = kvp.Value.getSet();
                    if (!set)
                    {
                         text[kvp.Key].decreaseTransparency();
                    }

                }
            }
        }
        else
        {
            //Debug.Log("Out of focus");
            foreach (KeyValuePair<int, LOD_TMP> kvp in text)
            {
                bool set = kvp.Value.getSet();
                if (set)
                {
                    text[kvp.Key].increaseTransparency();
                }

            }
        }
    }

    double calcDist(Vector3 obj1, Vector3 obj2)
    {
        double x = obj1.x - obj2.x;
        double y = obj1.y - obj2.y;
        double z = obj1.z - obj2.z;

        double sum = x * x + y * y + z * z;

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

    double getTextSize(TextMeshPro t)
    {
        Transform pt = t.transform.parent;
        double scale = t.fontSize * t.transform.localScale.x;
        while (pt != null)
        {
            scale *= pt.transform.localScale.x;
            pt = pt.parent;
        }

        return scale;
    }

    double getObjectSize(GameObject g)
    {
        Transform t = g.transform;
        Transform pt = t.transform.parent;
        double volume = t.transform.localScale.x * t.transform.localScale.y * t.transform.localScale.z;
        double scale = volume;
        while (pt != null)
        {
            volume = pt.transform.localScale.x * pt.transform.localScale.y * pt.transform.localScale.z;
            scale *= volume;
            pt = pt.parent;
        }

        return scale;
    }


    //function to determin scale to dist ratio
    double getRatio(double scale, double dist)
    {
        return ((1000 * scale) / (5 * dist));
    }

    void GroupTMP(List<TextMeshPro> allText)
    {
        allText.Sort(new TextSizeFirst());
        double oldSize = getTextSize(allText[0]);
        int groupId = 1;
        List<TextMeshPro> temp = new List<TextMeshPro>();
        foreach (TextMeshPro t in allText)
        {
            double newSize = getTextSize(t);
            double diff = oldSize - newSize;
            if (diff > .01)
            {
                text[groupId] = new LOD_TMP(groupId, 0, temp, false);
                temp = new List<TextMeshPro>();
                groupId += 1;
            }

            temp.Add(t);
            oldSize = newSize;
        }

        text[groupId] = new LOD_TMP(groupId, 0, temp, false);

        foreach (KeyValuePair<int, LOD_TMP> kvp in text)
        {
            Debug.Log("kvp.Key: " + kvp.Key);
            List<TextMeshPro> list = kvp.Value.getText();
            foreach (TextMeshPro t in list)
            {
                double s = getTextSize(t);
                Debug.Log(t + " fontSize: " + t.fontSize.ToString() + " global size:" + s.ToString());
            }
        }

    }

    double GetMedian(List<GameObject> AllObjects)
    {
        int length = AllObjects.Count;
        double median;
        if(length % 2 == 0)
        {
            int index1 = length / 2;
            int index2 = length - 1;
            GameObject obj1 = AllObjects[index1];
            GameObject obj2 = AllObjects[index2];
            double val1 = getObjectSize(obj1);
            double val2 = getObjectSize(obj2);
            median = (val1 + val2) / 2;
        }
        else
        {
            int index1 = length / 2;
            GameObject obj1 = AllObjects[index1];
            double val1 = getObjectSize(obj1);
            median = val1;
        }

        return median;

    }

    double StandardDeviation(List<GameObject> AllObjects)
    {
        double avg = 0;

        foreach(GameObject g in AllObjects)
        {
            double v = getObjectSize(g);
            avg += v;
        }
        avg = avg / (AllObjects.Count);

        double sum = 0;
        foreach (GameObject g in AllObjects)
        {
            Transform t = g.transform;
            double v = getObjectSize(g);
            sum += ((v - avg) * (v - avg));
        }

        return Math.Sqrt(sum/AllObjects.Count);
    }


    void GroupObjects(List<GameObject> AllObjects)
    {
        //need to group structure vs detail, put very large objects in [0]
        //figure out how to determine structure vs detail

        AllObjects.Sort(new VolumeFirst());
        objectMedian = GetMedian(AllObjects);
        double sdv = StandardDeviation(AllObjects);

        Debug.Log("MEDIAN: " + objectMedian.ToString() + "\nSDV: " + sdv.ToString());


        double oldV = getObjectSize(AllObjects[0]);
        double largestV = oldV;
        
        int groupId = 0;
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject g in AllObjects)
        {
            double newV = getObjectSize(g);
            double diff = oldV - newV;
            if (groupId == 0 && diff > sdv)
            {
                objects[groupId] = new LOD_Obj(groupId, 0, temp, false);
                temp = new List<GameObject>();
                groupId += 1;
            }
            else if (groupId != 0 && diff > 0)
            {
                objects[groupId] = new LOD_Obj(groupId, 0, temp, false);
                temp = new List<GameObject>();
                groupId += 1;
            }

            temp.Add(g);
            oldV = newV;
        }

        objects[groupId] =  new LOD_Obj(groupId, 0, temp, false);

        foreach (KeyValuePair<int, LOD_Obj> kvp in objects)
        {
            Debug.Log("kvp.Key: " + kvp.Key);
            List<GameObject> list = kvp.Value.getObjects();
            foreach (GameObject g in list)
            {
                double v = getObjectSize(g);
                Debug.Log(g + " " + " " + v.ToString());
            }
        }

    }


    //inteaction only at highest LOD
    void GroupInteraction(List<Interactable> allInteraction)
    {
        interaction[1] = new LOD_Interact(1, 0, allInteraction, false);
    }

    public static void AddOnClick(Interactable interactable)
    {
        interactable.OnClick.AddListener(() => Debug.Log("Interactable clicked"));
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