using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Referee
{
    private Player leftPlayer;
    private Player rightPlayer;
    private Environment environment;

    public event EventHandler<RefereeEventArgs> RefereeEvent;

    public Referee(Player leftPlayer, Player rightPlayer, Environment environment)
    {
        this.leftPlayer = leftPlayer;
        this.rightPlayer = rightPlayer;
        this.environment = environment;
    }

    public void Tick()
    {
        leftPlayer.Tick();
        rightPlayer.Tick();
        Debug.Log($"Left action: {leftPlayer.Current_action}");
        Debug.Log($"Right action: {rightPlayer.Current_action}");
        ResolveEvents(leftPlayer, rightPlayer, environment);
    }

    private void ResolveEvents(Player leftPlayer, Player rightPlayer, Environment environment)
    {
        // game logic
        if (environment.Position <= 0) //left player is oob
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.ENVIRONEMNT, EventSource.LEFT, RefereeEventType.OutOfBounds));
        } else if (environment.Position >= 100)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.ENVIRONEMNT, EventSource.RIGHT, RefereeEventType.OutOfBounds));
        }

        //check game over
        if (environment.LeftPlayerTime <= 0) 
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.ENVIRONEMNT, EventSource.LEFT, RefereeEventType.Win));
        }
        else if (environment.RightPlayerTime <= 0)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.ENVIRONEMNT, EventSource.RIGHT, RefereeEventType.Win));
        }

        ResolveActionEvents(leftPlayer, rightPlayer);



    }


    private void ResolveActionEvents(Player leftPlayer, Player rightPlayer)
    {
        //blocking shove
        if (leftPlayer.Current_action == ActionState.SHOVING && rightPlayer.Current_action == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushBlock));
        } 
        else if(rightPlayer.Current_action == ActionState.SHOVING && leftPlayer.Current_action == ActionState.BLOCKING) {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushBlock));
        }

        //blocking push
        else if (leftPlayer.Current_action == ActionState.PUSHING && rightPlayer.Current_action == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushBlock));
        }
        else if (rightPlayer.Current_action == ActionState.PUSHING && leftPlayer.Current_action == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushBlock));
        }

        //dodge shove
        else if (leftPlayer.Current_action == ActionState.SHOVING && rightPlayer.Current_action == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.ShoveDodge));
        }
        else if (rightPlayer.Current_action == ActionState.SHOVING && leftPlayer.Current_action == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.ShoveDodge));
        }

        //dodge push
        else if (leftPlayer.Current_action == ActionState.PUSHING && rightPlayer.Current_action == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushDodge));
        }
        else if (rightPlayer.Current_action == ActionState.PUSHING && leftPlayer.Current_action == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushDodge));
        }

        //shove push
        else if (leftPlayer.Current_action == ActionState.PUSHING && rightPlayer.Current_action == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushShove));
        }
        else if (rightPlayer.Current_action == ActionState.PUSHING && leftPlayer.Current_action == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushShove));
        }

        //shove shove 
        else if (rightPlayer.Current_action == ActionState.SHOVING && leftPlayer.Current_action == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.ShoveShove));
        }
        //push push
        else if (rightPlayer.Current_action == ActionState.PUSHING && leftPlayer.Current_action == ActionState.PUSHING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushPush));
        }
        //block block
        else if (rightPlayer.Current_action == ActionState.BLOCKING && leftPlayer.Current_action == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.BlockBlock));
        }
        //dodge dodge
        else if (rightPlayer.Current_action == ActionState.DODGING && leftPlayer.Current_action == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.DodgeDodge));
        }


        //shove idle
        else if (leftPlayer.Current_action == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.ShoveContact));
        }
        else if (rightPlayer.Current_action == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.ShoveContact));
        }

        //push idle
        else if (leftPlayer.Current_action == ActionState.PUSHING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushContact));
        }
        else if (rightPlayer.Current_action == ActionState.PUSHING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushContact));
        }

        //dodge idle
        else if (leftPlayer.Current_action == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.Dodge));
        }
        else if (rightPlayer.Current_action == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.Dodge));
        }
        //block idle
        else if (leftPlayer.Current_action == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.Block));
        }
        else if (rightPlayer.Current_action == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.Dodge));
        }
        //idle idle
        else if (leftPlayer.Current_action == ActionState.IDLE)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.Idle));
        }
 
    }

    protected virtual void OnRefereeEvent(RefereeEventArgs e)
    {
        RefereeEvent?.Invoke(this, e);
    }
}
