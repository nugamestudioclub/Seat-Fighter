using System;

[Serializable]
public class Environment
{
    private int position;
    public int Position
    {
        get => position;
        set
        {
            position = value;
            OnEnvironmentChange(
                new EnvironmentEventArgs(
                    EventSource.ENVIRONEMNT,
                    EnvironmentEventType.PosistionChange,
                    value));
        }
    }

    private int leftPlayerTime;
    public int LeftPlayerTime
    {
        get => leftPlayerTime;
        set
        {
            leftPlayerTime = value;
            OnEnvironmentChange(
                new EnvironmentEventArgs(
                    EventSource.ENVIRONEMNT,
                    EnvironmentEventType.LeftPlayerTimerUpdated,
                    value));
        }
    }

    private int rightPlayerTime;
    public int RightPlayerTime
    {
        get => rightPlayerTime;
        set
        {
            rightPlayerTime = value;
            OnEnvironmentChange(
                new EnvironmentEventArgs(
                    EventSource.ENVIRONEMNT,
                    EnvironmentEventType.RightPlayerTimerUpdated,
                    value));
        }
    }

    public event EventHandler<EnvironmentEventArgs> EnvironmentChangeEvent;

    public Environment(int position, int leftPlayerTime, int rightPlayerTime)
    {
        Position = position;
        LeftPlayerTime = leftPlayerTime;
        RightPlayerTime = rightPlayerTime;
    }

    protected virtual void OnEnvironmentChange(EnvironmentEventArgs e)
    {
        EnvironmentChangeEvent?.Invoke(this, e);
    }

}
