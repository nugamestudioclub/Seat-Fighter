using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

[Serializable]
public struct AiAction
{

    public readonly static AiAction EMPTY = new AiAction(Action.None, 0);

    public AiAction(Action action, int delay)
    {
        Action = action;
        Delay = delay;
    }

    public Action Action;
    public int Delay;
    

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