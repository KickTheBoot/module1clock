using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelloPohja : MonoBehaviour
{
    public static KelloPohja Master;
    public static DateTime currentTime;
    public delegate void OnMasterDestroy();
    public static event OnMasterDestroy OnMasterDestroyed;

    public float DifferenceToUTC = 0;


    public static float time;
    public static float Hour, Min;

    // Start is called before the first frame update
    void Start()
    {
        if (Master == null)
        {
            Master = this;
            Debug.Log($"Master clock: {Master.gameObject.name}");
        }
    }

    // Update is called once per frame
    public void Update()
    {
        //Master clock updates the time for all the other clocks to use
        if (Master == this)
        {
            currentTime = DateTime.UtcNow;
            time = ((float)(currentTime.Hour) * 3600) + ((float)currentTime.Minute * 60) + ((float)currentTime.Second) + ((float)currentTime.Millisecond / 1000);
            Min = (time / 60);
            Hour = (time / 3600);

            Debug.Log($"Time: {Mathf.Floor(Hour % 24)}:{Mathf.Floor(Min % 60)}:{Mathf.Floor(time) % 60}");
        }
        updatetimedisplay();
    }

    public void OnDestroy()
    {
        if (Master == this)
        {
            Master = null;
            //Null check to avoid null reference exceptions
            if (OnMasterDestroyed != null) OnMasterDestroyed.Invoke();

        }
        else OnMasterDestroyed -= this.OnMasterDestruction;
    }

    public void OnMasterDestruction()
    {
        //Assign this clock as the master clock if one's not assigned yet
        if (Master == null)
        {
            OnMasterDestroyed -= this.OnMasterDestruction;
            Master = this;
            Debug.Log($"Master clock: {Master.gameObject.name}");
        }
    }

    public virtual void updatetimedisplay()
    {

    }
}
