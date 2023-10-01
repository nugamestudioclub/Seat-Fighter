using System;
using UnityEngine;

public class PlayerTickEventArgs : EventArgs
{
    public readonly EventSource sender;
    public readonly ActionFrameData actionFrameData;

    public PlayerTickEventArgs(EventSource sender, ActionFrameData actionFrameData)
    {
        this.sender = sender;
        this.actionFrameData = actionFrameData;
    }
}
