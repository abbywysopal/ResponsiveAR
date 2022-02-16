using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

/*
 * TODO:
 *  2. LOD3 not going away 
 *  3. work on scale
 *  4. increase size of letters in LOD1
 * 
 * works for fast delta, not for slow delta
 * major glitching
 * need to figure out a way to do both scale and dis together
 * 
 * 
 */

public class DeviceMetrics : MonoBehaviour
{

    /*
     * 
     * Hololens 1 vs 2
     * 
     * Display resolution	
     *   1280 × 720 (per eye)	
     *   2048 × 1080 px (per eye)
     * Holographic density	
     *  >2.5K radiants (light points per radian)	
     *  >2.5K radiants (light points per radian)
     *  Field of view (FOV)	
     *      34°	
     *      52°
     */


    public Vector2 resolution = new Vector2(2048, 1080); //Holographic resolution	2k 3:2 light engines
    public double density = 2588; //Holographic density	>2.5k radiants (pixels per inch)
    public Vector2 aspect_ratio = new Vector2(3, 2);
    public int field_of_view = 52;
    public float screen_width;
    public float screen_height;
    public int screen_count = 2;

    void Start()
    {
        screen_width = resolution.x;
        screen_height = resolution.y;
    }
}