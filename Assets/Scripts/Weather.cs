using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.Networking;
using System.IO;

public class Weather : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> times;
    [SerializeField] List<TextMeshProUGUI> periods;
    [SerializeField] List<TextMeshProUGUI> temps;
    [SerializeField] TextMeshProUGUI location;
    [SerializeField] TextMeshProUGUI main_temp;
    [SerializeField] TextMeshProUGUI min_temp;
    [SerializeField] TextMeshProUGUI max_temp;
    [SerializeField] TextMeshProUGUI wind_speed;
    [SerializeField] TextMeshProUGUI condition;
    [SerializeField] TextMeshProUGUI uv_index;
    [SerializeField] TextMeshProUGUI humidity;
    [SerializeField] TextMeshProUGUI visibility;
    [SerializeField] TextMeshProUGUI feels;
    [SerializeField] Slider slider;

    private float timer;
    float minutesBetweenUpdate;

    string apiKey = "ff0e108b6d244c1ba87190238223003";
    string apiCall_forcast = "http://api.weatherapi.com/v1/forecast.json?key=";
    double lat = 34.413359;
    double lon = -119.844820;
    WeatherInfo weather;
    public static UserStudyTask study;

    // Start is called before the first frame update
    void Start()
    {
        //can do IP address?
        string dt = "&days=2&aqi=no&alerts=no";
        apiCall_forcast += apiKey + "&q=" + lat.ToString() + "," + lon.ToString() + dt;

        Debug.Log(apiCall_forcast);
        minutesBetweenUpdate = 10f;
        timer = 0;

        GetWeatherInfo();

    }
    
    public static void setTask(UserStudyTask s)
    {
        study = s;
    }


    public string getMainTemp()
    {
        return main_temp.text;
    }

    public string getMinTemp()
    {
        return min_temp.text;
    }

    public string getLocation()
    {
        return  location.text.ToLower();
    }

    private IEnumerator GetWeatherInfo()
    {
        var www = new UnityWebRequest(apiCall_forcast)
        {
            downloadHandler = new DownloadHandlerBuffer()
        };

        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //error
            yield break;
        }

        weather = JsonUtility.FromJson<WeatherInfo>(www.downloadHandler.text);
        setUp(weather.location.name, weather.current.temp_f, weather.forecast.forecastday[0].day.mintemp_f, weather.forecast.forecastday[0].day.maxtemp_f, weather.current.wind_mph, weather.current.condition.text, weather.current.uv, weather.current.humidity, weather.current.feelslike_f, weather.current.vis_miles);
        int hour = DateTime.Now.Hour;
        setTimePeriodTemp(hour);
    }

    // Update is called once per frame
    void Update()
    {
        //if ip found then
        if (timer <= 0)
        {
            StartCoroutine(GetWeatherInfo());
            timer = minutesBetweenUpdate * 60;
        }
        else
        {
            timer -= Time.deltaTime;
        }

    }

    void setUp(string loc, float t, float min, float max, float s, string c, float index, float perc, float f, float v)
    {
        location.text = loc;
        int t_int = (int)t;
        main_temp.text = t_int.ToString();
        t_int = (int)min;
        slider.minValue = t_int;
        min_temp.text = t_int.ToString();
        t_int = (int)max;
        slider.maxValue = t_int;
        max_temp.text = t_int.ToString();
        wind_speed.text = "Wind: " + s.ToString() + "mph";
        condition.text = c;
        uv_index.text = "UV Index: " + index.ToString();
        humidity.text = "Humidity: " + perc.ToString() + "%" ;
        feels.text = "Feels Like " + f.ToString();
        visibility.text = "Visibility: " + v.ToString() + "mi";

        float ratio = t / max;
        Debug.Log("slider " + t);
        
        slider.value = t;
    }

    void setTimePeriodTemp(int hour)
    {
        for (int i = 0; i < times.Count; i++)
        {
            int newTime = hour + i;
            int temp = Int32.Parse(main_temp.text);

            if(newTime < 24)
            {
                 temp = (int)weather.forecast.forecastday[0].hour[newTime].temp_f;
            }
            else
            {
                temp = (int)weather.forecast.forecastday[1].hour[newTime - 24].temp_f;
            }

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
            temps[i].text = temp.ToString();
        }


    }

    public void clicked(string input)
    {
        Debug.Log("clicked: " + input);
    }
}

[Serializable]
class WeatherInfo
{
    public Location location; //working
    public Current current;
    public Forecast forecast;

}
[Serializable]
class Location
{
    public string name;
    public string region;
/*    public string country;
	public float lat;
    public float lon;
	public string tz_id;
    public float localtime_epoch;
	public string localtime;*/
}

[Serializable]
class Forecast
{
    public ForecastDay [] forecastday;
}

[Serializable]
class Current
{
/*    public float last_updated_epoch;
    public DateTime last_updated;*/
	public float temp_c;
    public float temp_f;
    public Condition condition;
    public float wind_mph;
    public float humidity;
    public float feelslike_f;
    public float vis_miles;
    public float uv;
/*    public int is_day;
    public Condition condition;
    public float wind_mph;
    public float wind_kph;
    public float wind_degree;
	public string wind_dir;
	public float pressure_mb;
	public float pressure_in;
	public float precip_mm;
	public float precip_in;
	public float humidity;
	public float cloud;
	public float feelslike_c;
	public float feelslike_f;
	public float vis_km;
	public float vis_miles;
	public float uv;
	public float gust_mph;
	public float gust_kph;*/
}

[Serializable]
class Condition
{
    public string text;
    public string icon;
    public int code;
}

[Serializable]
class ForecastDay
{
/*    public string date;
    public int date_epoch;*/
    public Day day;
    public Hour[] hour;
}

[Serializable]
class Day
{
/*    public float maxtemp_c;*/
    public float maxtemp_f;
/*    public float mintemp_c;*/
    public float mintemp_f;
/*    public float avgtemp_c;
    public float avgtemp_f;
    public float maxwind_mph;
    public float maxwind_kph;
    public float totalprecip_mm;
    public float totalprecip_in;
    public float avgvis_km;
    public float avgvis_miles;
    public float avghumidity;
    public float daily_will_it_rain;
    public float daily_chance_of_rain;*/
    
}

[Serializable]
class Hour
{
/*    public int time_epoch;
    public DateTime time;*/
    public float temp_c;
    public float temp_f;

}