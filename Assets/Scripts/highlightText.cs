using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Microsoft.MixedReality.Toolkit.UI;

public class highlightText : MonoBehaviour
{
    Color32 color;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void highlightUGUI(TextMeshProUGUI textUGUI)
    {
        color = textUGUI.color;
        byte a = (byte)(10);
        textUGUI.color = new Color32(color[0], color[1], color[2], a);
    }

    public void unhighlightUGUI(TextMeshProUGUI textUGUI)
    {
        color = textUGUI.color;
        byte a = (byte)(255);
        textUGUI.color = new Color32(color[0], color[1], color[2], a);
    }

    public void highlightTMP(TextMeshPro textTMP)
    {
        color = textTMP.color;
        textTMP.color = new Color32(255, 255, 0, 255);
    }
}
