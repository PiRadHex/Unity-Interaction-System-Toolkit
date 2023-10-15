using UnityEngine;
using System.Collections;
using System;

public class Clock : MonoBehaviour
{  
    public float clockSpeed = 1.0f;
    
    public Transform pointerSeconds;
    public Transform pointerMinutes;
    public Transform pointerHours;

    private DateTime dateTime;

    void Update() 
    {
        dateTime = DateTime.Now;

        float rotationSeconds = (360.0f / 60.0f)  * (dateTime.Second);
        float rotationMinutes = (360.0f / 60.0f)  * dateTime.Minute;
        float rotationHours   = ((360.0f / 12.0f) * dateTime.Hour) + ((360.0f / (60.0f * 12.0f)) * dateTime.Minute);

        pointerSeconds.transform.localEulerAngles = new Vector3(0, rotationSeconds, 0);
        pointerMinutes.transform.localEulerAngles = new Vector3(0, rotationMinutes, 0);
        pointerHours.transform.localEulerAngles   = new Vector3(0, rotationHours, 0);
    }

}
