//A simple timer class
public class Timer
{
    public float interval;
    public float lastTick {get; private set;}

    public Timer(float interval, float currentTime)
    {
        this.interval = interval;
        this.lastTick = currentTime;
    }

    //Returns true when the time specified by interval has elapsed since the last tick
    public bool Tick(float currentTime)
    {
        if (currentTime >= (lastTick + interval))
        {
            lastTick = currentTime;
            return true;
        }
        else return false;
    }

}