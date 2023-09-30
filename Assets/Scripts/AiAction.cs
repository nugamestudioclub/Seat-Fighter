using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public struct AiAction
{

    public readonly static AiAction EMPTY = new AiAction(Action.None, 0);

    public Action Action { private set; get; }
    public int Delay { private set; get; }

    public AiAction(Action action, int delay)
    {
        this.Action = action;
        this.Delay = delay;
    }

    public AiAction(Action action) : this(action, 0)
    {

    }

    public bool checkDelay()
    {
        if (Delay > 0) {
            Delay -= 1;
            return false;
        }
        return true;
    }

    public bool Equals(AiAction other)
    {
        return Action == other.Action && Delay == other.Delay;
    }
}