using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.UI;

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

public class ResponsiveDesign : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    Collider collider;
    
    //IDictionary<int, List<TextMeshPro>> text = new Dictionary<int, List<TextMeshPro>>();
    //IDictionary<int, List<GameObject>> objects = new Dictionary<int, List<GameObject>>();
    //IDictionary<int, List<Interactable>> interaction = new Dictionary<int, List<Interactable>>();
    List<int> LOD = new List<int>();
    IDictionary<int, double> LODRatio = new Dictionary<int, double>();
    IDictionary<int, bool> LODSet = new Dictionary<int, bool>();

    IDictionary<int, LOD_TMP> text = new Dictionary<int, LOD_TMP>();
    IDictionary<int, LOD_Obj> objects = new Dictionary<int, LOD_Obj>();
    IDictionary<int, List<Interactable>> interaction = new Dictionary<int, List<Interactable>>();

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
            GroupTMP(allText);
        }

        if (allObjects.Count > 0)
        {
            GroupObjects(allObjects);
        }

        if (allInteraction.Count > 0)
        {
            GroupInteraction(allInteraction);
            foreach(KeyValuePair<int, List<Interactable>> kvp in interaction)
            {
                Debug.Log("LOD" + kvp.Key + ":");
                foreach(Interactable t in kvp.Value)
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
        //setUpLODInteraction(r,s,d);

    }


    //1 point = .0003527 m
    //12 points = .004233
    void setUpLODText(double r, double s, double d)
    {

        for(int i = 1; i <= text.Count; i++)
        {
            double textSize = text[i].getTextSize(parent.transform.localScale.x);
            double readable = (double).11;
            double size = readable / textSize;
            double result = size * r;
            text[i].setRatio(result);
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
        Transform t = parent.transform;
        float parent_volume = (t.localScale.x*t.localScale.y*t.localScale.z);
        double largestObj = objects[0].getSize(parent.transform) / parent_volume;
        objects[0].setRatio(r / largestObj);
        Debug.Log("LOD0: ");
        Debug.Log("largestObj: " + largestObj.ToString());
        Debug.Log("ratio: " + r.ToString());
        
        for(int i = 1; i <= objects.Count; i++)
        {
            double sizeObj = objects[i].getSize(parent.transform) / parent_volume;
            double result = r / sizeObj;
            objects[i].setRatio(result);
            Debug.Log("LOD: " + i.ToString());
            Debug.Log("sizeObj: " + sizeObj.ToString());
            Debug.Log("result: " + result.ToString());
            Debug.Log("ratio: " + r.ToString());
        }
    }

    void setLOD(int i, bool val)
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
            foreach (Interactable interact in interaction[i])
            {
                interact.SetInteractive(val);
            }
        }
        LODSet[i] = val;
    }

    void continuousFunction(double ratio)
    {

        foreach(KeyValuePair<int, double> kvp in LODRatio)
        {
            double r = kvp.Value;
            int i = kvp.Key;
            if (ratio < r)
            {
                setLOD(i, false);
                if (text.ContainsKey(i))
                {
                    text[i].decreaseTransparency();
                }
            }
            else
            {
                if (text.ContainsKey(i))
                {
                    text[i].increaseTransparency();
                }
                setLOD(i, true);
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

    double averageTextSize(List<TextMeshPro> objs, double scale)
    {
        double totalSize = 0;
        double totalObjs = 0f;
        foreach (TextMeshPro obj in objs)
        {
            totalObjs += 1f;
            totalSize += obj.fontSize * scale;
        }

        return totalSize / totalObjs;

    }


    //function to determin scale to dist ratio
    double getRatio(double scale, double dist)
    {
        return ((1000 * scale) / (5 * dist));
    }

    void GroupTMP(List<TextMeshPro> allText)
    {
        allText.Sort(new TextSizeFirst());
        float oldSize = allText[0].fontSize * allText[0].transform.localScale.x;
        int groupId = 1;
        List<TextMeshPro> temp = new List<TextMeshPro>();
        foreach (TextMeshPro t in allText)
        {
            float newSize = t.fontSize * t.transform.localScale.x;
            float diff = oldSize - newSize;
            if (diff > .01)
            {
                text[groupId] = new LOD_TMP(groupId, 0, temp, false);
                temp = new List<TextMeshPro>();
                LOD.Add(groupId);
                groupId += 1;
            }

            temp.Add(t);
            oldSize = newSize;
        }

        text[groupId] = new LOD_TMP(groupId, 0, temp, false);
        LOD.Add(groupId);

        foreach (KeyValuePair<int, LOD_TMP> kvp in text)
        {
            Debug.Log("kvp.Key: " + kvp.Key);
            List<TextMeshPro> list = kvp.Value.getText();
            foreach (TextMeshPro t in list)
            {
                float s = t.fontSize * t.transform.localScale.x * parent.transform.localScale.x;
                Debug.Log(t + " " + t.fontSize.ToString() + " " + s.ToString());
            }
        }

    }


    void GroupObjects(List<GameObject> AllObjects)
    {
        AllObjects.Sort(new VolumeFirst());

        float oldV = AllObjects[0].transform.localScale.x * AllObjects[0].transform.localScale.y * AllObjects[0].transform.localScale.z;
        int groupId = 0;
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject g in AllObjects)
        {
            Transform t = g.transform;
            float newV = t.localScale.x * t.localScale.y * t.localScale.z;
            float diff = oldV - newV;
            if (diff > .01)
            {
                objects[groupId] = new LOD_Obj(groupId, 0, temp, false);
                temp = new List<GameObject>();
                if (!LOD.Contains(groupId))
                {
                    LOD.Add(groupId);
                }
                groupId += 1;
            }

            temp.Add(g);
            oldV = newV;
        }

        objects[groupId] =  new LOD_Obj(groupId, 0, temp, false);
        if (!LOD.Contains(groupId))
        {
            LOD.Add(groupId);
        }

        foreach (KeyValuePair<int, LOD_Obj> kvp in objects)
        {
            Debug.Log("kvp.Key: " + kvp.Key);
            List<GameObject> list = kvp.Value.getObjects();
            foreach (GameObject g in list)
            {
                Transform t = g.transform;
                float v = t.localScale.x * t.localScale.y * t.localScale.z;
                Debug.Log(g + " " + " " + v.ToString());
            }
        }

    }


    //inteaction only at highest LOD
    void GroupInteraction(List<Interactable> allInteraction)
    {
        int index = LOD.Count;
        LOD.Add(index);
        interaction[index] = allInteraction;

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