using System;

public class PlayerEventArgs : EventArgs
{
    public readonly EventSource sender;
    public readonly Action action;
    public readonly int stamina;
    public readonly int maxStamina;

    public PlayerEventArgs(EventSource sender, Action action, int stamina, int maxStamina)
    {
        this.sender = sender;
        this.action = action;
        this.stamina = stamina;
        this.maxStamina = maxStamina;
    }
}
