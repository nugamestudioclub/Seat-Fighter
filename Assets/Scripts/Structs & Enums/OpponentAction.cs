using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAction
{

    public readonly ActionState Action;
    public readonly int Startup;

    public OpponentAction(ActionState action, int startup)
    {
        Action = action;
        Startup = startup;
    }
}

