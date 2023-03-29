using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kello : MonoBehaviour
{
    //a static Master clock to lower the amount of function calls that every other clock needs to make
    static Kello Master;

    //Static fields will only be updated by the master clock
    private static event OnMasterDestroy OnMasterDestroyed; //invoked when the master clock object is destroyed

    //the current time in seconds
    static float time;
    static float Hour, Min;

    static DateTime currentTime;

    float LastTick;

    public AnimationCurve SecondHandAngleCurve;
    private delegate void OnMasterDestroy();

    private AudioSource Ticker;


    public AudioClip HourChime;

    Timer timer1, chimeTimer;

    [SerializeField]
    private int ChimeCount;
    private int lastChime;

    //The transforms for the objectss representing hands
    public Transform HourH, MinH, SecH;

    public float DifferenceToUTC = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (TryGetComponent<AudioSource>(out Ticker)) Debug.Log($"{gameObject.name}: AudioSource Found");
        else Debug.Log($"{gameObject.name}: No AudioSource Found");
        currentTime = DateTime.UtcNow;
        time = ((float)(currentTime.Hour) * 3600) + ((float)currentTime.Minute * 60) + ((float)currentTime.Second) + ((float)currentTime.Millisecond / 1000);
        timer1 = new Timer(1, Mathf.Floor(time));
        chimeTimer = new Timer(2, Mathf.Floor(time));

        //Assign this clock as the master clock if one's not assigned yet
        if (Master == null)
        {
            Master = this;
            Debug.Log($"Master clock: {Master.gameObject.name}");
        }
        else OnMasterDestroyed += this.OnMasterDestruction;

    }

    // Update is called once per frame
    void Update()
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

        //Clock ticking timing condition
        if (timer1.Tick(time))
        {
            LastTick = Mathf.Floor(timer1.lastTick);

            if (Ticker) Ticker.Play();

            if (Mathf.Floor(Min % 60) == 0 && MathF.Floor(lastChime) != Hour)
            {
                ChimeCount = (((int)Mathf.Floor(Hour) + (int)Mathf.Floor(DifferenceToUTC)) % 12) + 1;
            }
        }

        if (HourChime)
        {
            if (chimeTimer.Tick(time) && ChimeCount > 0)
            {
                Ticker.PlayOneShot(HourChime);
                ChimeCount -= 1;
            }
        }

        HourH.rotation = Quaternion.Euler(0, 0, (Hour + DifferenceToUTC) * 30);
        MinH.rotation = Quaternion.Euler(0, 0, Min * 6);
        SecH.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(LastTick * 6, (LastTick + 1) * 6, SecondHandAngleCurve.Evaluate(time - LastTick)));
    }

    void OnDestroy()
    {
        if (Master == this)
        {
            Master = null;
            //Null check to avoid null reference exceptions
            if (OnMasterDestroyed != null) OnMasterDestroyed.Invoke();

        }
        else OnMasterDestroyed -= this.OnMasterDestruction;
    }

    void OnMasterDestruction()
    {
        //Assign this clock as the master clock if one's not assigned yet
        if (Master == null)
        {
            OnMasterDestroyed -= this.OnMasterDestruction;
            Master = this;
            Debug.Log($"Master clock: {Master.gameObject.name}");
        }
    }
}
