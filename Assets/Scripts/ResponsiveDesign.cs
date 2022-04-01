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
 *  1. compare textsize of applications to textsize on computer
 *  2. run weather app checking scroll
 *  3. find bug in interaction, should not be able to interact when all text is not present
 *  
 *  BUGS:
 * 
 */
[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Collider))]
public class ResponsiveDesign : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    [SerializeField]
    Collider collider;

    double objectMedian;
    double readibilityRatio = (8f / .35f) * 100;
    double highestRatio = (double).0;


    IDictionary<int, LOD_TMP> text = new Dictionary<int, LOD_TMP>();
    IDictionary<int, LOD_TMP_GUI> text_gui = new Dictionary<int, LOD_TMP_GUI>();
    IDictionary<int, LOD_Obj> objects = new Dictionary<int, LOD_Obj>();
    IDictionary<int, LOD_Interact> interaction = new Dictionary<int, LOD_Interact>();

    // Start is called before the first frame update
    void Start()
    { 

        SetUp();
        var headPosition = Camera.main.transform.position;
        //double disToHead = calcDist(Camera.main.transform.position, parent.transform.position);
        double distance = Math.Abs(dist(parent.transform, Camera.main.transform));
        double scale = Math.Abs(parent.transform.localScale.x);
        double ratio = getRatio(scale, distance);
        setUpLOD(ratio, scale, distance);

        continuousFunction(ratio, scale, distance);
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
        //double disToHead = calcDist(Camera.main.transform.position, parent.transform.position);
        double distance = Math.Abs(dist(parent.transform, Camera.main.transform));
        double scale = Math.Abs(parent.transform.localScale.x);
        double ratio = getRatio(scale, distance);
        //Debug.Log("Ratio: " + ratio.ToString());
        continuousFunction(ratio, scale, distance);
        gazeFunction();

    }


    void SetUp()
    {
        TextMeshPro[] allTMP;
        TextMeshProUGUI[] allTMP_GUI;
        Interactable[] interactables;
        Transform[] allTransforms;

        allTMP = parent.GetComponentsInChildren<TextMeshPro>();
        allTMP_GUI = parent.GetComponentsInChildren<TextMeshProUGUI>();
        interactables = parent.GetComponentsInChildren<Interactable>();
        allTransforms = parent.GetComponentsInChildren<Transform>();

        List<GameObject> allObjects = new List<GameObject>();
        List<TextMeshPro> allText = new List<TextMeshPro>(allTMP);
        List<TextMeshProUGUI> allText_GUI = new List<TextMeshProUGUI>(allTMP_GUI);
        List<Interactable> allInteraction = new List<Interactable>(interactables);

        allTransforms = parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in allTransforms)
        {
            if (child.childCount == 0)
            {
                TextMeshPro tmp = child.GetComponent<TextMeshPro>();
                Interactable i = child.GetComponent<Interactable>();
                TextMeshProUGUI tmp_gui = child.GetComponent<TextMeshProUGUI>();

                if (tmp == null && i == null && tmp_gui == null)
                {
                    allObjects.Add(child.gameObject);
                }

                child.gameObject.SetActive(false);
            }
            /*
            else
            {
                child.gameObject.SetActive(true);
            }
            */
        }

/*        Debug.Log("allTransforms");
        Debug.Log(allTransforms.Length);
        Debug.Log("allObjects");
        Debug.Log(allObjects.Count);
        Debug.Log("allText");
        Debug.Log(allText.Count);
        Debug.Log("allText_GUI");
        Debug.Log(allText_GUI.Count);
        Debug.Log("allInteraction");
        Debug.Log(allInteraction.Count);
*/


        if (allText.Count > 0)
        {
            GroupTMP(allText);
        }

        if (allText_GUI.Count > 0)
        {
            GroupTMP_GUI(allText_GUI);
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

    void setUpLOD(double r, double s, double d)
    {
        setUpLODText(r,s,d);
        setUpLODObjects(r,s,d);
        setUpLODInteraction(r,s,d);

        foreach (KeyValuePair<int, LOD_TMP> kvp in text)
        {
            //Debug.Log("TMP LOD" + kvp.Key + ":");
            //Debug.Log("Ratio:" + kvp.Value.getRatio().ToString());
            List<TextMeshPro> list = kvp.Value.getText();
            /*
            foreach (TextMeshPro t in list)
            {
                Debug.Log(t + " " + t.text);
            }
            */
        }
        foreach (KeyValuePair<int, LOD_TMP_GUI> kvp in text_gui)
        {
            //Debug.Log("TMP_GUI LOD" + kvp.Key + ":");
            //Debug.Log("Ratio:" + kvp.Value.getRatio().ToString());
            List<TextMeshProUGUI> list = kvp.Value.getText();
            /*
            foreach (TextMeshProUGUI t in list)
            {
                Debug.Log(t + " " + t.text);
            }
            */
        }
        foreach (KeyValuePair<int, LOD_Obj> kvp in objects)
        {
            //Debug.Log("OBJ LOD" + kvp.Key + ":");
            //Debug.Log("Ratio: " + kvp.Value.getRatio().ToString());
            List<GameObject> list = kvp.Value.getObjects();
            /*
            foreach (GameObject t in list)
            {
                Debug.Log(t);
            }
            */
        }
        foreach (KeyValuePair<int, LOD_Interact> kvp in interaction)
        {
            //Debug.Log("INT LOD" + kvp.Key + ":");
            //Debug.Log("Ratio: " + kvp.Value.getRatio().ToString());
            List<Interactable> list = kvp.Value.getInteractables();
            /*
            foreach (Interactable t in list)
            {
                Debug.Log(t);
            }
            */
        }
    }


    //1 point = .0003527 m
    //12 points = .004233
    //TODO: setSet(false);?
    void setUpLODText(double r, double s, double d)
    {

        float fontRatio = .04f / 0.35f; //https://www.sciencebuddies.org/science-fair-projects/science-fair/display-board-fonts
        //Debug.Log("fontRatio: " + fontRatio);
        //Debug.Log("ratio: " + r);
        for (int i = 1; i <= text.Count; i++)
        {
            double textSize = text[i].getTextSize();
            double size = fontRatio / textSize;
            double result = size * r;
            if (i == 1) { result = 0; }
            text[i].setRatio(result);
            if(result > highestRatio) { highestRatio = result; }
            /*
            Debug.Log("LOD: " + i.ToString());
            Debug.Log("textSize: " + textSize.ToString());
            Debug.Log("size: " + size.ToString());
            Debug.Log("result: " + result.ToString());
            Debug.Log("ratio: " + r.ToString());'
            */

        }

        //TMPUGUI incorrect sizes
        fontRatio *= .2f;//something weird going on with canvas and UI text
        //Debug.Log("fontRatio: " + fontRatio);

        for (int i = 1; i <= text_gui.Count; i++)
        {
            double textSize = text_gui[i].getTextSize();
            double size = fontRatio / textSize;
            double result = size * r;
            if (i == 1) { result = 0; }
            text_gui[i].setRatio(result);
            if (result > highestRatio) { highestRatio = result; }

            //Debug.Log("LOD: " + i.ToString());
            //Debug.Log("textSize: " + textSize.ToString());
            //Debug.Log("size: " + size.ToString());
            //Debug.Log("result: " + result.ToString());
            //Debug.Log("ratio: " + r.ToString());

        }

    }

    void setUpLODObjects(double r, double s, double d)
    {
        //TODO: FIX THIS FUNCTION
        //percentages?
        //these are determined by volume 3D scale, lettering is 1D scale

        double objectRatio = .007f / 0.35f; //https://www.sciencebuddies.org/science-fair-projects/science-fair/display-board-fonts
        objectRatio *= .01f;
        objectRatio *= .01f;


        //Debug.Log("fontRatio: " + objectRatio);
        //Debug.Log("ratio: " + r);

        Transform t = parent.transform;
        double largestObj = objects[0].getLocalSize();
        double smallestObj = objects[objects.Count - 1].getLocalSize();
        objects[0].setRatio(0);

        //Debug.Log("objectMedian: " + objectMedian.ToString());
        //Debug.Log("scale: " + s.ToString());

        for(int i = 1; i < objects.Count; i++)
        {
/*            int scaler = objects.Count - i;*/

            double sizeObj = objects[i].getLocalSize();
            double result = r * (objectRatio / sizeObj);
/*            result /= scaler;*/

            if(result > highestRatio)
            {
                result = highestRatio;
            }
            objects[i].setRatio(result);
            //Debug.Log("LOD: " + i.ToString());
            //Debug.Log("sizeObj: " + sizeObj.ToString());
            //Debug.Log("result: " + result.ToString());
            //Debug.Log("ratio: " + r.ToString());
        }
    }

    void setUpLODInteraction(double r, double s, double d)
    {
        foreach(KeyValuePair<int, LOD_Interact> kvp in interaction)
        {
            //Debug.Log("higestRatio" + highestRatio.ToString());
            kvp.Value.setRatio(highestRatio);
            kvp.Value.setUpInteraction();
        }

    }

    void continuousFunction(double ratio, double scale, double distance)
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
        foreach (KeyValuePair<int, LOD_TMP_GUI> kvp in text_gui)
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
            string message = "interaction ";
            if (ratio < r)
            {
                kvp.Value.setLOD(false);
                message += "off";
            }
            else
            {
                kvp.Value.setLOD(true);
                 message += "on";
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

    double dist(Transform t1, Transform t2)
    {
        return (t1.position - t2.position).magnitude;
    }

    //.35m -> 12points
    //.7m 16 points
    //1.5m -> 32 points

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

    double getTextSize(TextMeshProUGUI t)
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
        return ((scale) / (dist));
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
            if (diff > 0.0)
            {
                text[groupId] = new LOD_TMP(groupId, 0, temp, false);
                temp = new List<TextMeshPro>();
                groupId += 1;
            }

            temp.Add(t);
            oldSize = newSize;
        }

        text[groupId] = new LOD_TMP(groupId, 0, temp, false);

    /*       foreach (KeyValuePair<int, LOD_TMP> kvp in text)
               {
            //Debug.Log("kvp.Key: " + kvp.Key);
            List<TextMeshPro> list = kvp.Value.getText();
            foreach (TextMeshPro t in list)
            {
                double s = getTextSize(t);
                //Debug.Log(t + " fontSize: " + t.fontSize.ToString() + " global size:" + s.ToString());
            }
        }
    */

    }

    void GroupTMP_GUI(List<TextMeshProUGUI> allText)
    {
        allText.Sort(new TextSizeFirst_GUI());
        double oldSize = getTextSize(allText[0]);
        int groupId = 1;
        List<TextMeshProUGUI> temp = new List<TextMeshProUGUI>();
        foreach (TextMeshProUGUI t in allText)
        {
            double newSize = getTextSize(t);
            double diff = oldSize - newSize;
            if (diff > 0.0)
            {
                text_gui[groupId] = new LOD_TMP_GUI(groupId, 0, temp, false);
                temp = new List<TextMeshProUGUI>();
                groupId += 1;
            }

            temp.Add(t);
            oldSize = newSize;
        }

        text_gui[groupId] = new LOD_TMP_GUI(groupId, 0, temp, false);

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

/*        Debug.Log("MEDIAN: " + objectMedian.ToString() + "\nSDV: " + sdv.ToString());*/


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

/*        foreach (KeyValuePair<int, LOD_Obj> kvp in objects)
        {
            Debug.Log("kvp.Key: " + kvp.Key);
            List<GameObject> list = kvp.Value.getObjects();
            foreach (GameObject g in list)
            {
                double v = getObjectSize(g);
                Debug.Log(g + " " + " " + v.ToString());
            }
        }
*/

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