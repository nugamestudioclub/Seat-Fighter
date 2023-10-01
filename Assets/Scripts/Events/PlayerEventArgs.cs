using System;

public class PlayerEventArgs : EventArgs
{
    public readonly EventSource sender;
    public readonly Action action;
    public readonly int stamina;

    public PlayerEventArgs(EventSource sender, Action action, int stamina)
    {
        this.sender = sender;
        this.action = action;
        this.stamina = stamina;
    }
}
