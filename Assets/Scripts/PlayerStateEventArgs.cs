using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateEventArgs : EventArgs
{
    public readonly EventSource sender;
    public readonly PlayerState state;

    public PlayerStateEventArgs(EventSource sender, PlayerState state)
    {
        this.sender = sender;
        this.state = state;
    }
}