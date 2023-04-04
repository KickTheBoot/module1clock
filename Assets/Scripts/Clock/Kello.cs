using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kello : KelloPohja
{

    float LastTick;

    public AnimationCurve SecondHandAngleCurve;

    private AudioSource Ticker;


    public AudioClip HourChime;

    Timer timer1, chimeTimer;

    [SerializeField]
    private int ChimeCount;
    private int lastChime;

    //The transforms for the objectss representing hands
    public Transform HourH, MinH, SecH;

    


    // Start is called before the first frame update
    void Start()
    {
        
        if (TryGetComponent<AudioSource>(out Ticker)) Debug.Log($"{gameObject.name}: AudioSource Found");
        else Debug.Log($"{gameObject.name}: No AudioSource Found");
        
        time = ((float)(currentTime.Hour) * 3600) + ((float)currentTime.Minute * 60) + ((float)currentTime.Second) + ((float)currentTime.Millisecond / 1000);
        timer1 = new Timer(1, Mathf.Floor(time));
        chimeTimer = new Timer(2, time);

        //Assign this clock as the master clock if one's not assigned yet
        if (Master == null)
        {
            Master = this;
            currentTime = DateTime.UtcNow;
            Debug.Log($"Master clock: {Master.gameObject.name}");
        }
        else OnMasterDestroyed += this.OnMasterDestruction;

    }

    public override void updatetimedisplay()
    {
        if (timer1.Tick(Mathf.Floor(time)))
        {
            LastTick = timer1.lastTick;

            if (Ticker) Ticker.Play();

            if (Mathf.Floor(Min % 60) == 0 && MathF.Floor(lastChime) != Hour)
            {
                lastChime = (int)Hour;
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
        SecH.rotation = Quaternion.Euler(0, 0, Mathf.LerpUnclamped(LastTick * 6, (LastTick+1) * 6, SecondHandAngleCurve.Evaluate(time - LastTick)));
    }
}
