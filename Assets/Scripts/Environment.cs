using System;
using Unity.VisualScripting.FullSerializer;

public class Environment
{
    public EnvironmentConfig Config { get; private set; }
    private int position;
    public int Position
    {
        get => position;
        set
        {
            position = Math.Max(-Config.edgeDistance, 
                Math.Min(value, Config.edgeDistance + Config.armrestWidth));
            OnEnvironmentChange(
                new EnvironmentEventArgs(
                    EventSource.ENVIRONEMNT,
                    EnvironmentEventType.PosistionChange,
                    value,
                    Config.armrestWidth));
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

    public Environment(EnvironmentConfig config, int maxLeftPlayerTime, int maxRightPlayerTime)
    {
        this.Config = config;
        Position = config.startingPositon;
        this.maxLeftPlayerTime = maxLeftPlayerTime;
        this.maxRightPlayerTime = maxRightPlayerTime;
        LeftPlayerTime = maxLeftPlayerTime;
        RightPlayerTime = maxRightPlayerTime;
    }

    public void Start()
    {
        Position = Config.startingPositon;
        RightPlayerTime = maxRightPlayerTime;
        LeftPlayerTime = maxLeftPlayerTime;
    }
    protected virtual void OnEnvironmentChange(EnvironmentEventArgs e)
    {
        EnvironmentChangeEvent?.Invoke(this, e);
    }

}
