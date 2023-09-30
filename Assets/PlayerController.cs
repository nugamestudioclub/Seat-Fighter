using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XInput;
using UnityEngine.InputSystem;

public class PlayerController : IActionProvider
{
    [SerializeField]
    private ActionObject push;

    Player player;

    public Action GetNextAction()
    {
        Action a = Action.None;
        if (player.Current_action == Action_state.IDLE)
        {
            // TODO poll user input
        }
        return a;
    }
}
