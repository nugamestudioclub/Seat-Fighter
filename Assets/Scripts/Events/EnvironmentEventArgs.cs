using System;

public class EnvironmentEventArgs : EventArgs
{
    public readonly EventSource sender;
    public readonly EnvironmentEventType type;
    public readonly int value;

    public EnvironmentEventArgs(EventSource sender, EnvironmentEventType type, int value)
    {
        this.sender = sender;
        this.type = type;
        this.value = value;
    }
}
