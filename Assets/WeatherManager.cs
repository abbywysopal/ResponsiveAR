using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using SimpleJSON;

public class WeatherManager : MonoBehaviour
{
    public string apiKey;
    public string currentWeatherApi = "api.openweathermap.org/data/2.5/weather?";
    [Header("UI")]
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI location;
    public TextMeshProUGUI mainWeather;
    public TextMeshProUGUI description;
    public TextMeshProUGUI temp;
    public TextMeshProUGUI feels_like;
    public TextMeshProUGUI temp_min;
    public TextMeshProUGUI temp_max;
    public TextMeshProUGUI pressure;
    public TextMeshProUGUI humidity;
    public TextMeshProUGUI windspeed;
    private LocationInfo lastLocation;
    void Start()
    {
        StartCoroutine(FetchLocationData());
    }
    private IEnumerator FetchLocationData()
    {
        Debug.Log("FetchLocationData");

        Debug.Log(Input.location.isEnabledByUser);
        Debug.Log(Input.location);
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location service is not enabled!");
            yield break;
        }
        // Start service before querying location
        Input.location.Start();
        // Wait until service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log(maxWait);
            maxWait--;
        }
        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            statusText.text = "Location Timed out";
            Debug.Log("Location Timed out");
            yield break;
        }
        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            statusText.text = "Unable to determine device location";
            Debug.Log("Unable to determine device location");
            yield break;
        }
        else
        {
            lastLocation = Input.location.lastData;
            UpdateWeatherData();
        }
        Input.location.Stop();
    }
    private void UpdateWeatherData()
    {
        Debug.Log("UpdateWeatherData");
        StartCoroutine(FetchWeatherDataFromApi(lastLocation.latitude.ToString(), lastLocation.longitude.ToString()));
    }
    private IEnumerator FetchWeatherDataFromApi(string latitude, string longitude)
    {
        Debug.Log("FetchWeatherDataFromApi");
        string url = currentWeatherApi + "lat=" + latitude + "&lon=" + longitude + "&appid=" + apiKey + "&units=metric";
        UnityWebRequest fetchWeatherRequest = UnityWebRequest.Get(url);
        yield return fetchWeatherRequest.SendWebRequest();
        if (fetchWeatherRequest.isNetworkError || fetchWeatherRequest.isHttpError)
        {
            //Check and print error
            Debug.Log("ERROR");
            statusText.text = fetchWeatherRequest.error;
        }
        else
        {
            Debug.Log("ELSE");
            Debug.Log(fetchWeatherRequest.downloadHandler.text);
            var response = JSON.Parse(fetchWeatherRequest.downloadHandler.text);
            Debug.Log(response["name"]);
            location.text = response["name"];
            mainWeather.text = response["weather"][0]["main"];
            description.text = response["weather"][0]["description"];
            temp.text = response["main"]["temp"] + " C";
            feels_like.text = "Feels like " + response["main"]["feels_like"] + " C";
            temp_min.text = "Min is " + response["main"]["temp_min"] + " C";
            temp_max.text = "Max is " + response["main"]["temp_max"] + " C";
            pressure.text = "Pressure is " + response["main"]["pressure"] + " Pa";
            humidity.text = response["main"]["humidity"] + " % Humidity";
            windspeed.text = "Windspeed is " + response["wind"]["speed"] + " Km/h";
        }
    }
}