using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField]
    GameObject hoursPivot;
    [SerializeField]
    GameObject minutesPivot;
    [SerializeField]
    GameObject secondsPivot;

    const float hoursToDegrees = 30f, minutesToDegrees = 6f, secondToDegrees=6f;
    private void Awake() {
        //Debug.Log(DateTime.Now.Hour);
        var time = DateTime.Now;
        hoursPivot.transform.localRotation = Quaternion.Euler(90f + hoursToDegrees * time.Hour, 0, -90);
        minutesPivot.transform.localRotation = Quaternion.Euler(90f + minutesToDegrees * time.Minute, 0, -90);
        secondsPivot.transform.localRotation = Quaternion.Euler(90f + secondToDegrees * time.Second, 0, -90);
    }
    private void Update()
    {
        // TimeSpan time = DateTime.Now.TimeOfDay;
        var time = DateTime.Now;
        hoursPivot.transform.localRotation = Quaternion.Euler(90f + hoursToDegrees * time.Hour, 0, -90);
        minutesPivot.transform.localRotation = Quaternion.Euler(90f + minutesToDegrees * time.Minute, 0, -90);
        secondsPivot.transform.localRotation = Quaternion.Euler(90f + secondToDegrees * time.Second, 0, -90);
    }
}
