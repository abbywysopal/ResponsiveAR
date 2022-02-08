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
    List<TextMeshPro> LOD2objs = new List<TextMeshPro>();

    [SerializeField]
    GameObject LOD3parent;

    [SerializeField]
    List<GameObject> LOD3objs = new List<GameObject>();

    [SerializeField]
    int LOD1;
    [SerializeField]
    int transitionLOD2;
    int LOD2;
    [SerializeField]
    int LOD3;

    bool LOD1set = true;
    bool LOD2set = false;
    bool LOD3set = false;



    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
        Debug.Log("Scale: " + parent.transform.localScale);
        LOD1parent.SetActive(true);
        LOD2parent.SetActive(false);
        LOD3parent.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if(transform.localScale.x >= transitionLOD2 & !LOD2set){

            LOD2parent.SetActive(true);
            increaseTransparency(LOD2objs);
        }

        if(transform.localScale.x >= LOD2 & !LOD2set){
            setTransparency(LOD2objs, 255);
            LOD2set = true;
        }

        if(transform.localScale.x >= LOD3 & !LOD3set){
            LOD3parent.SetActive(true);
            LOD3set = true;
        }


    }

    void increaseTransparency(List<TextMeshPro> objs){
        foreach (TextMeshPro obj in objs){
            Color32 color = obj.faceColor;
            byte a = (byte)(color[3] *2);
            obj.faceColor = new Color32(color[0], color[1], color[2], a);
        }
    }

    void setTransparency(List<TextMeshPro> objs, byte a){
        foreach (TextMeshPro obj in objs){
            Color32 color = obj.faceColor;
            obj.faceColor = new Color32(color[0], color[1], color[2], a);
        }

    }
}
