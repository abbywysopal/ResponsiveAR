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

public class AutoSetUp : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    SphereCollider collider;

    GameObject LOD1parent;

    GameObject LOD2parent;
    List<TextMeshPro> LOD2text = new List<TextMeshPro>();
    List<GameObject> LOD2objs = new List<GameObject>();

    GameObject LOD3parent;
    List<GameObject> LOD3objs = new List<GameObject>();
    List<TextMeshPro> LOD3text = new List<TextMeshPro>();

    double LOD1 = 2;
    double LOD2 = 3.2;
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

    
    IDictionary<int, List<TextMeshPro>> text = new Dictionary<int, List<TextMeshPro>>();
    IDictionary<int, List<GameObject>> objects = new Dictionary<int, List<GameObject>>();
    IDictionary<int, List<Interactable>> interaction = new Dictionary<int, List<Interactable>>();
    List<int> LOD = new List<int>();
    IDictionary<int, double> LODRatio = new Dictionary<int, double>();
    IDictionary<int, bool> LODSet = new Dictionary<int, bool>();



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

        /*  
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
        */

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
        GameObject[] objects;
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
            Debug.Log(child);

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

        foreach (GameObject g in allObjects)
        {
            Debug.Log(g);
        }

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
        }

    }


    //1 point = .0003527 m
    //12 points = .004233
    void setUpLOD(double r, double s, double d)
    {
        LODRatio.Add(0, 0);
        setLOD(0, true);
        LODRatio.Add(1, r / 2);
        setLOD(1, true);
        double initail = ((double).11 / getTextSize(1)) * r;

        for (int i = 2; i < LOD.Count; i++){
            double textSize = getTextSize(i);
            double readable = (double).11;
            double size = readable / textSize;
            double result = size * r + initail;
            LODRatio.Add(i, result);
            Debug.Log("LOD: " + i.ToString());
            Debug.Log("textSize: " + textSize.ToString());
            Debug.Log("size: " + size.ToString());
            Debug.Log("result: " + result.ToString());
            Debug.Log("ratio: " + r.ToString());
        }
    }

    void setLOD(int i, bool val)
    {

        if (text.ContainsKey(i))
        {
            foreach(TextMeshPro t in text[i])
            {
                t.gameObject.SetActive(val);
            }
        }
        if (objects.ContainsKey(i))
        {
            foreach (GameObject g in objects[i])
            {
                g.SetActive(val);
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
                setLOD(i, true);
                disableObjects(objects[i]);
                decreaseTransparency(text[i]);
            }
            else
            {
                enableObjects(objects[i]);
                increaseTransparency(text[i]);
                setLOD(i, false);
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
                foreach (KeyValuePair<int, bool> kvp in LODSet)
                {
                    if (kvp.Value)
                    {
                        increaseTransparency(text[kvp.Key]);
                    }

                }
            }
            else
            {
                //Debug.Log("Hit something else");
                //Debug.Log(hitInfo.collider);
                foreach (KeyValuePair<int, bool> kvp in LODSet)
                {
                    if (!kvp.Value)
                    {
                        decreaseTransparency(text[kvp.Key]);
                    }

                }
            }
        }
        else
        {
            //Debug.Log("Out of focus");
            foreach (KeyValuePair<int, bool> kvp in LODSet)
            {
                if (kvp.Value)
                {
                    decreaseTransparency(text[kvp.Key]);
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

    void checkIncreaseInLOD(double scale, double dist, double ratio)
    {
        double fontSize2 = averageTextSize(LOD2text, scale);
        Debug.Log("font size 2: " + fontSize2);
        double fontSize3 = averageTextSize(LOD3text, scale);

        if (ratio < 2)
        {
            Debug.Log("LOD1 set");
        }
        else if (ratio < 3)
        {
            //LOD2 transition
            Debug.Log("increaseTrans\n");
            LOD2parent.SetActive(true);
            disableObjects(LOD2objs);
            increaseTransparency(LOD2text);
        }
        else if (ratio < 3.6)
        {
            //LDO2 set
            increaseTransparency(LOD2text);//call a algo to increase transparency based on gaze alg
            enableObjects(LOD2objs);
            // LOD2set = true;
        }
        else if (ratio < 5)
        {
            //LOD3 transition
            setTransparency(LOD2text, 255);
            Debug.Log("increaseTrans\n");
            LOD3parent.SetActive(true);
            disableObjects(LOD3objs);
            increaseTransparency(LOD3text);//call a algo to increase transparency based on gaze alg
        }
        else if (ratio < 6.7)
        {
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

        if (ratio < 1.5)
        {
            Debug.Log("LOD1 set");
            setTransparency(LOD2text, 0);
        }
        else if (ratio < 2.8)
        {
            //LDO2 unset
            Debug.Log("LOD2 unset");
            decreaseTransparency(LOD2text);
            disableObjects(LOD2objs);
            // LOD2set = false;
        }
        else if (ratio < 3.4)
        {
            //LOD2 transition
            Debug.Log("LOD2 trans dec");
            decreaseTransparency(LOD2text);
            setTransparency(LOD3text, 0);
            LOD3parent.SetActive(false);
        }
        else if (ratio < 4.8)
        {
            //LOD3 unset
            decreaseTransparency(LOD3text);
            disableObjects(LOD3objs);
            // LOD3set = false;
        }
        else if (ratio < 6.5)
        {
            //LOD3 transition
            decreaseTransparency(LOD3text);
        }
    }

    void increaseTransparency(List<TextMeshPro> objs)
    {
        if (objs[0].color[3] < 255)
        {
            foreach (TextMeshPro obj in objs)
            {
                Color32 color = obj.color;
                byte a = (byte)(color[3] + 1);
                if (color[3] + 1 >= 255)
                {
                    a = 255;
                }
                obj.color = new Color32(color[0], color[1], color[2], a);
            }
        }
    }

    void decreaseTransparency(List<TextMeshPro> objs)
    {
        if (objs[0].color[3] > 0)
        {
            foreach (TextMeshPro obj in objs)
            {
                Color32 color = obj.color;
                byte a = (byte)(color[3] - 1);
                if (color[3] - 1 <= 0)
                {
                    a = 0;
                }
                obj.color = new Color32(color[0], color[1], color[2], a);
            }
        }
    }

    void setTransparency(List<TextMeshPro> objs, byte a)
    {
        foreach (TextMeshPro obj in objs)
        {
            Color32 color = obj.color;
            obj.color = new Color32(color[0], color[1], color[2], a);
        }

    }

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

    double textSizeTMP(TextMeshPro obj)
    {
        return obj.fontSize * obj.transform.localScale.x;
    }


    void disableObjects(List<GameObject> objs)
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(false);
        }
    }

    void enableObjects(List<GameObject> objs)
    {
        foreach (GameObject obj in objs)
        {
            obj.SetActive(true);
        }
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
                text[groupId] = temp;
                temp = new List<TextMeshPro>();
                LOD.Add(groupId);
                groupId += 1;
            }

            temp.Add(t);
            oldSize = newSize;
        }

        text[groupId] = temp;
        LOD.Add(groupId);

        foreach (KeyValuePair<int, List<TextMeshPro>> kvp in text)
        {
            Debug.Log("kvp.Key: " + kvp.Key);
            foreach (TextMeshPro t in kvp.Value)
            {
                float s = t.fontSize * t.transform.localScale.x * parent.transform.localScale.x;
                Debug.Log(t + " " + t.fontSize.ToString() + " " + s.ToString());
            }
        }

    }

    double getTextSize(int index)
    {
        TextMeshPro t = text[index][0];
        return t.fontSize * t.transform.localScale.x * parent.transform.localScale.x;
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
                objects[groupId] = temp;
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

        objects[groupId] = temp;
        if (!LOD.Contains(groupId))
        {
            LOD.Add(groupId);
        }

        foreach (KeyValuePair<int, List<GameObject>> kvp in objects)
        {
            Debug.Log("kvp.Key: " + kvp.Key);
            foreach (GameObject g in kvp.Value)
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