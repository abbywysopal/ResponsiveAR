using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
    float LOD1;
    [SerializeField]
    float transitionLOD2;
    [SerializeField]
    float LOD2;
    [SerializeField]
    float LOD3;

    bool LOD1set = true;
    bool LOD2set = false;
    bool LOD3set = false;




    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Clock");
        Debug.Log("Scale: " + parent.transform.localScale);
        LOD1parent.SetActive(true);
        LOD2parent.SetActive(false);
        LOD3parent.SetActive(false);
        setTransparency(LOD2text, 10);
    }

    // Update is called once per frame
    void Update()
    {

        if(transform.localScale.x >= transitionLOD2 & !LOD2set){
            LOD2parent.SetActive(true);
            disableObjects(LOD2objs);
            increaseTransparency(LOD2text);
        }

        if(transform.localScale.x >= LOD2 & !LOD2set){
            setTransparency(LOD2text, 255);
            enableObjects(LOD2objs);
            LOD2set = true;
        }

        if(transform.localScale.x >= LOD3 & !LOD3set){
            LOD3parent.SetActive(true);
            LOD3set = true;
        }


    }

    void increaseTransparency(List<TextMeshPro> objs){
        foreach (TextMeshPro obj in objs){
            Color32 color = obj.color;
            byte a = (byte)(color[3] *2);
            obj.color = new Color32(color[0], color[1], color[2], a);

        }
    }

    void setTransparency(List<TextMeshPro> objs, byte a){
        foreach (TextMeshPro obj in objs){
            Color32 color = obj.color;
            obj.color = new Color32(color[0], color[1], color[2], a);
        }

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
     * 
     * 
     * 
     * 
     */
}
