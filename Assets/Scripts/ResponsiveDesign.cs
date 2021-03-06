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
 * 
 */

/*
 * Interaction: file:///C:/Program%20Files/Unity/Hub/Editor/2020.3.20f1/Editor/Data/Documentation/en/Manual/UIInteractionComponents.html
 */

/*
 * TODO:
 *  
 *  - not currently using gaze function, can add back 
 *  
 *  BUGS:
 * 
 */
[RequireComponent(typeof(Transform))]
public class ResponsiveDesign : MonoBehaviour
{
    [SerializeField]
    GameObject parent;

    double objectMedian;
    double ratio;
    double distance;
    double scale;


    IDictionary<int, LOD_TMP> text = new Dictionary<int, LOD_TMP>();
    IDictionary<int, LOD_TMP_GUI> text_gui = new Dictionary<int, LOD_TMP_GUI>();
    IDictionary<int, LOD_Obj> objects = new Dictionary<int, LOD_Obj>();
    IDictionary<int, LOD_Interact> interaction = new Dictionary<int, LOD_Interact>();
    IDictionary<int, LOD_Select> selection = new Dictionary<int, LOD_Select>();
    
    private static ResponsiveData responsiveData;

    // Start is called before the first frame update
    void Start()
    { 
        SetUp();
        var headPosition = Camera.main.transform.position;
        distance = Math.Abs(dist(parent.transform, Camera.main.transform));
        scale = Math.Abs(parent.transform.localScale.x);
        ratio = getRatio(scale, distance);
        setUpLOD(scale);

        continuousFunction(ratio);
    }

   
    // Update is called once per frame
    void Update()
    {
        /*       
            * when increasing transparency, we should check if size has changed
            * need to revert back when decrease in scale
        */

        var headPosition = Camera.main.transform.position;
        distance = Math.Abs(dist(parent.transform, Camera.main.transform));
        scale = Math.Abs(parent.transform.localScale.x);
        ratio = getRatio(scale, distance);
        continuousFunction(ratio);
        log_data();

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
        }

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

        float fontRatio = .09f / 1f; //https://www.sciencebuddies.org/science-fair-projects/science-fair/display-board-fonts

        for (int i = 0; i < text.Count; i++)
        {
            double textSize = text[i].getTextSize();
            double result = s * (fontRatio / textSize);
            text[i].setRatio(result);
        }

        fontRatio *= .1f;//TMProUGUI is 10* larger than TMPro

        for (int i = 0; i < text_gui.Count; i++)
        {
            double textSize = text_gui[i].getTextSize();
            double result = s * (fontRatio / textSize);
            text_gui[i].setRatio(result);
        }

        double objectRatio = .0045f / 1f; //objects do not need to be readable, so should appear at smaller sizes
        objectRatio *= .01f; //ratio is determined by volume unlike the size of text
        objectRatio *= .01f;

        for(int i = 0; i < objects.Count; i++)
        {
            double sizeObj = objects[i].getLocalSize();
            double result = s * (objectRatio / sizeObj);
            objects[i].setRatio(result);
        }

        objectRatio /= .01f;
        for (int i = 0; i < interaction.Count; i++)
        {
            double sizeObj = interaction[i].getLocalSize();
            double result = s * (objectRatio / sizeObj);
            interaction[i].setRatio(result);
        }

        objectRatio *= .01f;//UI 10* smaller than rest
        for (int i = 0; i < selection.Count; i++)
        {
            double sizeObj = selection[i].getLocalSize();
            double result = s * (objectRatio / sizeObj);
            selection[i].setRatio(result);
        }
    }

    public void disableResponsiveDesign()
    {
        foreach(KeyValuePair<int, LOD_TMP> kvp in text)
        {
            kvp.Value.setLOD(true);
            kvp.Value.setTransparency(255);
        }
        foreach (KeyValuePair<int, LOD_TMP_GUI> kvp in text_gui)
        {
            kvp.Value.setLOD(true);
            kvp.Value.setTransparency(255);
        }
        foreach (KeyValuePair<int, LOD_Obj> kvp in objects)
        {
            kvp.Value.setLOD(true);
        }
        foreach (KeyValuePair<int, LOD_Interact> kvp in interaction)
        {
            kvp.Value.setLOD(true);
        }
        foreach (KeyValuePair<int, LOD_Select> kvp in selection)
        {
            kvp.Value.setLOD(true);
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
        double scale = t.fontSize * t.transform.localScale.y;
        while (pt != null)
        {
            scale *= pt.transform.localScale.y;
            pt = pt.parent;
        }

        return scale;
    }

    double getTextSize(TextMeshProUGUI t)
    {
        Transform pt = t.transform.parent;
        double scale = t.fontSize * t.transform.localScale.y;
        while (pt != null)
        {
            scale *= pt.transform.localScale.y;
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

    }


    //inteaction only at highest LOD
    void GroupInteraction(List<Interactable> allInteraction)
    {

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

    private void log_data()
    {
        responsiveData = new ResponsiveData();
        responsiveData.distance = distance;
        responsiveData.ratio = ratio;

        responsiveData.scale = parent.transform.localScale;
        responsiveData.position = parent.transform.position;

        responsiveData.name = parent.name;
    }

    public static ResponsiveData GetResponsiveData()
    {
        return responsiveData;
    }

}

[System.Serializable]
public class ResponsiveData
{
    public string name;

    public Vector3 scale;
    public Vector3 position;

    public double ratio;
    public double distance;
}
