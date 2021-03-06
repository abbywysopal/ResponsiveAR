using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.UI;
using System.Globalization;

public class ResponsiveValues : MonoBehaviour
{

    [SerializeField]
    List<TextMeshPro> names;
    [SerializeField]
    List<TextMeshPro> types;
    [SerializeField]
    List<TextMeshPro> sizes;
/*    [SerializeField]
    List<TextMeshPro> curr_sizes;*/
    [SerializeField]
    List<TextMeshPro> actives;
    [SerializeField]
    List<TextMeshPro> ratios;
    [SerializeField]
    TextMeshPro scale;
    [SerializeField]
    TextMeshPro dist;
    [SerializeField]
    TextMeshPro p_ratio;

    [SerializeField]
    GameObject class_parent;

    IDictionary<int, LOD_TMP> text = new Dictionary<int, LOD_TMP>();
    IDictionary<int, LOD_TMP_GUI> text_gui = new Dictionary<int, LOD_TMP_GUI>();
    IDictionary<int, LOD_Obj> objects = new Dictionary<int, LOD_Obj>();
    IDictionary<int, LOD_Interact> interaction = new Dictionary<int, LOD_Interact>();
    IDictionary<int, LOD_Select> selection = new Dictionary<int, LOD_Select>();
    bool setUp;

    string specifier;
    CultureInfo culture;

    double parent_dist;

    // Start is called before the first frame update
    void Start()
    {
        setUp = false;
        specifier = "E02";
        culture = CultureInfo.CreateSpecificCulture("en-US");
    }

    // Update is called once per frame
    void Update()
    {
        text = class_parent.GetComponent<ResponsiveDesign>().getText();
        text_gui = class_parent.GetComponent<ResponsiveDesign>().getTextGUI();
        objects = class_parent.GetComponent<ResponsiveDesign>().getObjects();
        interaction = class_parent.GetComponent<ResponsiveDesign>().getInteraction();
        selection = class_parent.GetComponent<ResponsiveDesign>().getSelection();
        double parent_ratio = class_parent.GetComponent<ResponsiveDesign>().getRatio();
        p_ratio.text = "Ratio: " + parent_ratio.ToString(specifier, culture);
        parent_dist = class_parent.GetComponent<ResponsiveDesign>().getDist();
        dist.text = "Distance: " + parent_dist.ToString(specifier, culture);
        double parent_scale = class_parent.GetComponent<ResponsiveDesign>().getScale();
        scale.text = "Scale: " + parent_scale.ToString(specifier, culture);


        if (!setUp)
        {
            SetUpTableValues();
            setUp = true;
        }

        updateTableValues();
    }

    public void SetUpTableValues()
    {
        int count = 0;
        double size = 0;

        float ratio = .001f / 1f; //objects do not need to be readable, so should appear at smaller sizes
        ratio *= .01f; //ratio is determined by volume unlike the size of text
        ratio *= .01f;

        //objects then text, then interaction
        foreach (KeyValuePair<int, LOD_Obj> kvp in objects)
        {
            size = kvp.Value.getLocalSize();
            names[count].text = kvp.Value.getName();
            types[count].text = "Game Object";

            ratios[count].text = kvp.Value.getRatio().ToString();
            sizes[count].text = (size / parent_dist).ToString(specifier, culture);
            actives[count].text = kvp.Value.getSet().ToString();
            kvp.Value.getLOD();
            count += 1;
        }

        foreach (KeyValuePair<int, LOD_Interact> kvp in interaction)
        {
            size = kvp.Value.getLocalSize();
            names[count].text = kvp.Value.getName();
            types[count].text = "Interactable";
            ratios[count].text = kvp.Value.getRatio().ToString();
            sizes[count].text = (size/parent_dist).ToString(specifier, culture);
            actives[count].text = kvp.Value.getSet().ToString();
            count += 1;
        }

        
        foreach (KeyValuePair<int, LOD_Select> kvp in selection)
        {
            size = kvp.Value.getLocalSize();
            names[count].text = kvp.Value.getName();
            types[count].text = "Selectable";
            ratios[count].text = kvp.Value.getRatio().ToString();
            sizes[count].text = (size / parent_dist).ToString(specifier, culture);
            actives[count].text = kvp.Value.getSet().ToString();
            count += 1;
        }

        ratio = .009f / 1f;
       foreach (KeyValuePair<int, LOD_TMP_GUI> kvp in text_gui)
        {
            
            size = kvp.Value.getTextSize();
            names[count].text = kvp.Value.getName();
            types[count].text = "TMP_GUI";
            ratios[count].text = kvp.Value.getRatio().ToString();
            sizes[count].text = (size / parent_dist).ToString(specifier, culture);
            actives[count].text = kvp.Value.getSet().ToString();
            kvp.Value.getLOD();
            count += 1;
        }

        ratio *= 10;

        foreach (KeyValuePair<int, LOD_TMP> kvp in text)
        {
            size = kvp.Value.getTextSize();
            names[count].text = kvp.Value.getName();
            types[count].text = "TMP";
            ratios[count].text = kvp.Value.getRatio().ToString();
            sizes[count].text = (size / parent_dist).ToString(specifier, culture);
            actives[count].text = kvp.Value.getSet().ToString();
            count += 1;
            
        }

        for(int i = count; i < names.Count; i++)
        {
            names[i].transform.gameObject.SetActive(false);
            types[i].transform.gameObject.SetActive(false);
            sizes[i].transform.gameObject.SetActive(false);
            actives[i].transform.gameObject.SetActive(false);
            ratios[i].transform.gameObject.SetActive(false);
        }

    }

    public void updateTableValues()
    {
        int count = 0;
        double size = 0;
        //objects then text, then interaction
        foreach (KeyValuePair<int, LOD_Obj> kvp in objects)
        {
            size = kvp.Value.getLocalSize();
            actives[count].text = kvp.Value.getSet().ToString();
            sizes[count].text = (size).ToString(specifier, culture);
            count += 1;
        }

        foreach (KeyValuePair<int, LOD_Interact> kvp in interaction)
        {
            size = kvp.Value.getLocalSize();
            actives[count].text = kvp.Value.getSet().ToString();
            sizes[count].text = (size).ToString(specifier, culture);
            count += 1;
        }

        foreach (KeyValuePair<int, LOD_Select> kvp in selection)
        {
            size = kvp.Value.getLocalSize();
            actives[count].text = kvp.Value.getSet().ToString();
            sizes[count].text = (size).ToString(specifier, culture);
            count += 1;
        }

        foreach (KeyValuePair<int, LOD_TMP_GUI> kvp in text_gui)
        {
            size = kvp.Value.getTextSize();
            actives[count].text = kvp.Value.getSet().ToString();
            sizes[count].text = (size).ToString(specifier, culture);
            count += 1;
        }

        foreach (KeyValuePair<int, LOD_TMP> kvp in text)
        {
            size = kvp.Value.getTextSize();
            actives[count].text = kvp.Value.getSet().ToString();
            sizes[count].text = (size).ToString(specifier, culture);
            count += 1;

        }

    }
}