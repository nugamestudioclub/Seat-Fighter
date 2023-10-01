using System;
public class Environment
{
    private readonly int maxPosition;
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
                    value,
                    maxPosition));
        }
    }
    private readonly int maxLeftPlayerTime;

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
                    value,
                    maxLeftPlayerTime));
        }
    }
    private readonly int maxRightPlayerTime;
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
                    value,
                    maxRightPlayerTime));
        }
    }

    public event EventHandler<EnvironmentEventArgs> EnvironmentChangeEvent;

    public Environment(int position, int maxPosition, int maxLeftPlayerTime, int maxRightPlayerTime)
    {
        Position = position;
        this.maxPosition = maxPosition;
        this.maxLeftPlayerTime = maxLeftPlayerTime;
        this.maxRightPlayerTime = maxRightPlayerTime;
        LeftPlayerTime = maxLeftPlayerTime;
        RightPlayerTime = maxRightPlayerTime;
    }

    protected virtual void OnEnvironmentChange(EnvironmentEventArgs e)
    {
        EnvironmentChangeEvent?.Invoke(this, e);
    }

}
