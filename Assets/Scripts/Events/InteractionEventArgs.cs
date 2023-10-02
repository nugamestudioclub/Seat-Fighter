using System;

public class RefereeEventArgs : EventArgs
{
    public readonly EventSource sender;
    public readonly EventSource receiver;
    public readonly RefereeEventType type;

    public RefereeEventArgs(EventSource sender, EventSource reciever, RefereeEventType type)
    {
        this.sender = sender;
        this.receiver = reciever;
        this.type = type;
    }
}
