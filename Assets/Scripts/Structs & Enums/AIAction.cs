using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

[Serializable]
public struct AIAction
{

    public readonly static AIAction EMPTY = new AIAction(Action.None, 0);

    public AIAction(Action action, int delay)
    {
        Action = action;
        Delay = delay;
        Duration = 0;
    }

    public AIAction(Action action, int delay, int duration)
    {
        Action = action;
        Delay = delay;
        Duration = duration;
    }

    public Action Action;
    public int Delay;
    public int Duration;

    public bool CheckDelay()
    {
        if (Delay > 0)
        {
            Delay -= 1;
            return false;
        }
        return true;
    }

    public bool HoldAction()
    {
        switch (Action) {
            case Action.Block:
            case Action.Push:
                if (Duration > 0)
                {
                    Duration -= 1;
                    return true;
                }
                return false;
            default:
                return false;
        }
    }

    public bool Equals(AIAction other)
    {
        return Action == other.Action && Delay == other.Delay;
    }
}