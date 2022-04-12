using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageVariable : MonoBehaviour
{

    [SerializeField]
    bool value;
    [SerializeField]
    Sprite sprite1;
    [SerializeField]
    Sprite sprite2;
    [SerializeField]
    Image image;

    public void Toggle_Image()
    {
        value = !value;
        if (value)
        {
            image.sprite = sprite1;
        }
        else
        {
            image.sprite = sprite2;
        }

    }
}
