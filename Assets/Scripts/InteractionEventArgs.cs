using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEventArgs : EventArgs
{
    public readonly EventSource sender;
    public readonly EventSource reciever;
    public readonly EventType type;

    public InteractionEventArgs(EventSource sender, EventSource reciever, EventType type)
    {
        this.sender = sender;
        this.reciever = reciever;
        this.type = type;
    }
}
