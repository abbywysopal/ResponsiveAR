using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResponsiveObject : MonoBehaviour
{
    [SerializeField]
    GameObject parent;

    [SerializeField]
    GameObject LOD1parent;
    [SerializeField]
    GameObject LOD2parent;
    [SerializeField]
    List<GameObject> LOD2objs = new List<GameObject>();

    [SerializeField]
    GameObject LOD3parent = null;

    //[SerializeField]
    //List<GameObject> LOD3objs = new List<GameObject>();

    [SerializeField]
    int LOD1;
    [SerializeField]
    int LOD2;
    [SerializeField]
    int LOD3;

    bool LOD1set = true;
    bool LOD2set = false;
    bool LOD3set = false;
    
    //create multiple canvases that will be enabled and disabled depending on the level of item/size of canvas

    //need to find a way to transition between disable and enabled better
    void Start()
    {
        Debug.Log(parent.name);
        Debug.Log("Scale: " + parent.transform.localScale);
        Debug.Log("Distance: " + parent.transform.localPosition);
        LOD1parent.SetActive(true);
        LOD2parent.SetActive(false);
        LOD3parent.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.localScale.x >= LOD2 & !LOD2set){
            LOD2parent.SetActive(true);
            LOD2set = true;
        }

        if(LOD3parent != null)
        {
            if(transform.localScale.x >= LOD3 & !LOD3set){
                LOD3parent.SetActive(true);
                LOD3set = true;
            }
        }
    }


}
