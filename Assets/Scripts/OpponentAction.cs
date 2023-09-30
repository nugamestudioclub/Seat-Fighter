using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAction
{

    public readonly Action_state Action;
    public readonly int Startup;

    public OpponentAction(Action_state action, int startup)
    {
        Action = action;
        Startup = startup;
    }
}

