using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Weather : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> times;
    [SerializeField] List<TextMeshProUGUI> periods;
    [SerializeField] List<TextMeshProUGUI> temps;
    

    int min;
    int max;

    string apiKey = "ff0e108b6d244c1ba87190238223003";
    string apiCall_current = "http://api.weatherapi.com/v1/current.json?key=";
    string apiCall_forcast = "http://api.weatherapi.com/v1/forecast.json?key=";
    double lat = 34.413359;
    double lon = -119.844820;

    // Start is called before the first frame update
    void Start()
    {
        min = 54;
        max = 78;
        int hour = DateTime.Now.Hour;
        setTimeAndPeriod(hour);

        //can do IP address?

        apiCall_current += apiKey + "&q=" + lat.ToString() + "," + lon.ToString();
        apiCall_forcast += apiKey + "&q=" + lat.ToString() + "," + lon.ToString();
        Debug.Log(apiCall_current);


        //string dt = "dt=" + DateTime.Today.ToString("yyyy-MM-dd");
        string dt = "&days=1&aqi=no&alerts=no";
        apiCall_forcast += dt;
        Debug.Log(apiCall_forcast);

        //For future API 'dt' should be between 14 days and 300 days from today in the future in yyyy-MM-dd format (i.e. dt=2023-01-01)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float slider_val()
    {
        return ((float)min/max);
    }

    void setTimeAndPeriod(int hour)
    {

        for(int i = 0; i < times.Count; i++)
        {
            int newTime = hour + i;
            string tt = "AM";
            if (newTime >= 12)
            {
                tt = "PM";
            }

            periods[i].text = tt;

            if(newTime != 12)
            {
                newTime %= 12;
            }

            times[i].text = newTime.ToString();
        }


    }
}
