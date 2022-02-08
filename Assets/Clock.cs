using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Clock : MonoBehaviour
{
    [SerializeField]
    GameObject secondsHand;
    [SerializeField]
    GameObject minutesHand;
    [SerializeField]
    GameObject hoursHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DateTime currentTime = DateTime.Now;
        Debug.Log("seconds:" + currentTime.Second);
    

        float secondsDegree = -(currentTime.Second / 60f) *360f;
        Debug.Log("seconds:" + secondsDegree);
        secondsHand.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, secondsDegree));
        float minutesDegree = -(currentTime.Minute / 60f) *360f;
        Debug.Log("minutes:" + currentTime.Minute);
        Debug.Log("minutes:" + minutesDegree);
        minutesHand.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, minutesDegree));
        float hoursDegree = -(currentTime.Hour / 12f) *360f;
        hoursHand.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, hoursDegree));

    }
}
