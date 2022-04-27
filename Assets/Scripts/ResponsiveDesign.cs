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
 *      - not currently using gaze function, can add back 
 * 
 */

/*
 * Interaction: file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.20f1/Editor/Data/Documentation/en/Manual/UIInteractionComponents.html
 */

/*
 * TODO:
 *  
 *  BUGS:
 * 
 */
[RequireComponent(typeof(Transform))]
/*[RequireComponent(typeof(Collider))]*/
public class ResponsiveDesign : MonoBehaviour
{
    [SerializeField]
    GameObject parent;
    //[SerializeField]
    //Collider collider;

    double objectMedian;
    double ratio;
    double distance;
    double scale;


    IDictionary<int, LOD_TMP> text = new Dictionary<int, LOD_TMP>();
    IDictionary<int, LOD_TMP_GUI> text_gui = new Dictionary<int, LOD_TMP_GUI>();
    IDictionary<int, LOD_Obj> objects = new Dictionary<int, LOD_Obj>();
    IDictionary<int, LOD_Interact> interaction = new Dictionary<int, LOD_Interact>();
    IDictionary<int, LOD_Select> selection = new Dictionary<int, LOD_Select>();
    

    // Start is called before the first frame update
    void Start()
    { 
        SetUp();
        var headPosition = Camera.main.transform.position;
        //double disToHead = calcDist(Camera.main.transform.position, parent.transform.position);
        distance = Math.Abs(dist(parent.transform, Camera.main.transform));
        scale = Math.Abs(parent.transform.localScale.x);
        ratio = getRatio(scale, distance);
        setUpLOD(scale);

        continuousFunction(ratio);
        //gazeFunction();
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
        distance = Math.Abs(dist(parent.transform, Camera.main.transform));
        scale = Math.Abs(parent.transform.localScale.x);
        ratio = getRatio(scale, distance);
        //Debug.Log("Ratio: " + ratio.ToString());
        continuousFunction(ratio);
        //gazeFunction();

    }

    
    public IDictionary<int, LOD_TMP> getText()
    {
        return text;
    }
    public IDictionary<int, LOD_TMP_GUI> getTextGUI()
    {
        return text_gui;
    }
    public IDictionary<int, LOD_Obj> getObjects()
    {
        return objects;
    }
    public IDictionary<int, LOD_Interact> getInteraction()
    {
        return interaction;
    }
    public IDictionary<int, LOD_Select> getSelection()
    {
        return selection;
    }

    public double getRatio()
    {
        return ratio;
    }
    public double getDist()
    {
        return distance;
    }
    public double getScale()
    {
        return scale;
    }

    void SetUp()
    {
        TextMeshPro[] allTMP;
        TextMeshProUGUI[] allTMP_GUI;
        Interactable[] interactables;
        Transform[] allTransforms;
        Selectable[] selectables;

        allTMP = parent.GetComponentsInChildren<TextMeshPro>();
        allTMP_GUI = parent.GetComponentsInChildren<TextMeshProUGUI>();
        interactables = parent.GetComponentsInChildren<Interactable>();
        allTransforms = parent.GetComponentsInChildren<Transform>();
        selectables = parent.GetComponentsInChildren<Selectable>();

        List<GameObject> allObjects = new List<GameObject>();
        List<TextMeshPro> allText = new List<TextMeshPro>(allTMP);
        List<TextMeshProUGUI> allText_GUI = new List<TextMeshProUGUI>(allTMP_GUI);
        List<Interactable> allInteraction = new List<Interactable>(interactables);
        List<Selectable> allSelection = new List<Selectable>(selectables);

        allTransforms = parent.GetComponentsInChildren<Transform>();

        foreach (Transform child in allTransforms)
        {
            if (child.childCount == 0)
            {
                TextMeshPro tmp = child.GetComponent<TextMeshPro>();
                Interactable i = child.GetComponent<Interactable>();
                TextMeshProUGUI tmp_gui = child.GetComponent<TextMeshProUGUI>();
                Selectable s = child.GetComponent<Selectable>();

                if (tmp == null && i == null && tmp_gui == null && s == null)
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

        /*
        Debug.Log("allTransforms");
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

        if(allSelection.Count > 0)
        {
            GroupSelection(allSelection);
        }


    }

    void setUpLOD(double s)
    {
        //setUpLODText(s);
        //setUpLODObjects(s);

        float fontRatio = .09f / 1f; //https://www.sciencebuddies.org/science-fair-projects/science-fair/display-board-fonts

        //Debug.Log("fontRatio: " + fontRatio);
        //Debug.Log("ratio: " + r);
        for (int i = 0; i < text.Count; i++)
        {
            double textSize = text[i].getTextSize();
            double size = fontRatio / textSize;
            double result = size * s;
            text[i].setRatio(result);
/*            Debug.Log("textSize: " + textSize);
            Debug.Log("ratio: " + result);*/

        }

        fontRatio *= .1f;//TMProUGUI is 10* larger than TMPro
        //Debug.Log("fontRatio: " + fontRatio);

        for (int i = 0; i < text_gui.Count; i++)
        {
            double textSize = text_gui[i].getTextSize();
            double size = fontRatio / textSize;
            double result = size * s;
            text_gui[i].setRatio(result);

        }

        double objectRatio = .001f / 1f; //objects do not need to be readable, so should appear at smaller sizes
        objectRatio *= .01f; //ratio is determined by volume unlike the size of text
        objectRatio *= .01f;

        for(int i = 0; i < objects.Count; i++)
        {
            double sizeObj = objects[i].getLocalSize();
            double result = s * (objectRatio / sizeObj);
            objects[i].setRatio(result);
/*            Debug.Log("objSize: " + sizeObj);
            Debug.Log("ratio: " + result);*/
        }

        for (int i = 0; i < interaction.Count; i++)
        {
            double sizeObj = interaction[i].getLocalSize();
            double result = s * (objectRatio / sizeObj);
            interaction[i].setRatio(result);
/*            Debug.Log("objSize: " + sizeObj);
            Debug.Log("ratio: " + result);*/
        }

        objectRatio *= .01f;//UI 10* smaller than rest
        for (int i = 0; i < selection.Count; i++)
        {
            double sizeObj = selection[i].getLocalSize();
            double result = s * (objectRatio / sizeObj);
            selection[i].setRatio(result);
/*            Debug.Log("objSize: " + sizeObj);
            Debug.Log("ratio: " + result);*/
        }
    }


    void setUpLODText(double s)
    {

        float fontRatio = .05f / 1f; //https://www.sciencebuddies.org/science-fair-projects/science-fair/display-board-fonts

        //Debug.Log("fontRatio: " + fontRatio);
        //Debug.Log("ratio: " + r);
        for (int i = 0; i < text.Count; i++)
        {
            double textSize = text[i].getTextSize();
            double size = fontRatio / textSize;
            double result = size * s;
            text[i].setRatio(result);
/*            Debug.Log("textSize 2: " + textSize);
            Debug.Log("ratio: " + result);*/

        }

        fontRatio *= .1f;//TMProUGUI is 10* larger than TMPro
        //Debug.Log("fontRatio: " + fontRatio);

        for (int i = 0; i < text_gui.Count; i++)
        {
            double textSize = text_gui[i].getTextSize();
            double size = fontRatio / textSize;
            double result = size * s;
            text_gui[i].setRatio(result);

        }

    }

    void setUpLODObjects(double s)
    {

        double objectRatio = .005f / 1f; //objects do not need to be readable, so should appear at smaller sizes
        objectRatio *= .01f; //ratio is determined by volume unlike the size of text
        objectRatio *= .01f;

/*        objects[0].setRatio(0);*/

        for(int i = 0; i < objects.Count; i++)
        {
            double sizeObj = objects[i].getLocalSize();
            double result = s * (objectRatio / sizeObj);
            objects[i].setRatio(result);
        }

        for (int i = 0; i < interaction.Count; i++)
        {
            double sizeObj = interaction[i].getLocalSize();
            double result = s * (objectRatio / sizeObj);
            interaction[i].setRatio(result);
        }

        for (int i = 0; i < selection.Count; i++)
        {
            double sizeObj = selection[i].getLocalSize();
            double result = s * (objectRatio / sizeObj);
            selection[i].setRatio(result);
        }
    }


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
            if (ratio < r)
            {
                kvp.Value.setLOD(false);
            }
            else
            {
                kvp.Value.setLOD(true);
            }
        }
        foreach (KeyValuePair<int, LOD_Select> kvp in selection)
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

    /*
     * CAN USE THIS FUNCTION FOR text transparency changes
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
                         Debug.Log("call increase in gaze");
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
                     Debug.Log("call increase in gaze");
                    text[kvp.Key].increaseTransparency();
                }

            }
        }
    }
    */

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
        int groupId = 0;
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
                Debug.Log(t + " fontSize: " + t.fontSize.ToString() + " global size:" + s.ToString());
            }
        }
    */

    }

    void GroupTMP_GUI(List<TextMeshProUGUI> allText)
    {
        allText.Sort(new TextSizeFirst_GUI());
        double oldSize = getTextSize(allText[0]);
        int groupId = 0;
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


    void GroupObjects(List<GameObject> AllObjects)
    {
        //need to group structure vs detail, put very large objects in [0]
        //figure out how to determine structure vs detail

        AllObjects.Sort(new VolumeFirst());

        double oldV = getObjectSize(AllObjects[0]);
        
        int groupId = 0;
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject g in AllObjects)
        {
            double newV = getObjectSize(g);
            double diff = oldV - newV;
            if (diff > 0.0)
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

        //interaction[1] = new LOD_Interact(1, 0, allInteraction, false);

        allInteraction.Sort(new VolumeFirst_Interact());

        double oldV = getObjectSize(allInteraction[0].gameObject);

        int groupId = 0;
        List<Interactable> temp = new List<Interactable>();
        foreach (Interactable g in allInteraction)
        {
            double newV = getObjectSize(g.gameObject);
            double diff = oldV - newV;
            if (diff > 0.0)
            {
                interaction[groupId] = new LOD_Interact(groupId, 0, temp, false);
                temp = new List<Interactable>();
                groupId += 1;
            }

            temp.Add(g);
            oldV = newV;
        }

        interaction[groupId] = new LOD_Interact(groupId, 0, temp, false);
    }

    //selection only at highest LOD
    void GroupSelection(List<Selectable> allSelection)
    {
        //selection[1] = new LOD_Select(1, 0, allSelection, false);
        allSelection.Sort(new VolumeFirst_Select());

        double oldV = getObjectSize(allSelection[0].gameObject);

        int groupId = 0;
        List<Selectable> temp = new List<Selectable>();
        foreach (Selectable g in allSelection)
        {
            double newV = getObjectSize(g.gameObject);
            double diff = oldV - newV;
            if (diff > 0.0)
            {
                selection[groupId] = new LOD_Select(groupId, 0, temp, false);
                temp = new List<Selectable>();
                groupId += 1;
            }

            temp.Add(g);
            oldV = newV;
        }

        selection[groupId] = new LOD_Select(groupId, 0, temp, false);
    }

}