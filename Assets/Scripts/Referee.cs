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

    public event EventHandler<InteractionEventArgs> Interaction;

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
        if (environment.position <= 0) //left player is oob
        {
            OnInteraction(new InteractionEventArgs(EventSource.ENVIRONEMNT, EventSource.LEFT, EventType.OutOfBounds));
        } else if (environment.position >= 100)
        {
            OnInteraction(new InteractionEventArgs(EventSource.ENVIRONEMNT, EventSource.RIGHT, EventType.OutOfBounds));
        }

        //check game over
        if (environment.leftPlayerTime <= 0) 
        {
            OnInteraction(new InteractionEventArgs(EventSource.ENVIRONEMNT, EventSource.LEFT, EventType.Win));
        }
        else if (environment.rightPlayerTime <= 0)
        {
            OnInteraction(new InteractionEventArgs(EventSource.ENVIRONEMNT, EventSource.RIGHT, EventType.Win));
        }

        ResolveActionEvents(leftPlayer, rightPlayer);



    }


    private void ResolveActionEvents(Player leftPlayer, Player rightPlayer)
    {
        //blocking shove
        if (leftPlayer.Current_action == Action_state.SHOVING && rightPlayer.Current_action == Action_state.BLOCKING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.PushBlock));
        } 
        else if(rightPlayer.Current_action == Action_state.SHOVING && leftPlayer.Current_action == Action_state.BLOCKING) {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.PushBlock));
        }

        //blocking push
        else if (leftPlayer.Current_action == Action_state.PUSHING && rightPlayer.Current_action == Action_state.BLOCKING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.PushBlock));
        }
        else if (rightPlayer.Current_action == Action_state.PUSHING && leftPlayer.Current_action == Action_state.BLOCKING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.PushBlock));
        }

        //dodge shove
        else if (leftPlayer.Current_action == Action_state.SHOVING && rightPlayer.Current_action == Action_state.DODGING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.ShoveDodge));
        }
        else if (rightPlayer.Current_action == Action_state.SHOVING && leftPlayer.Current_action == Action_state.BLOCKING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.ShoveDodge));
        }

        //dodge push
        else if (leftPlayer.Current_action == Action_state.PUSHING && rightPlayer.Current_action == Action_state.DODGING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.PushDodge));
        }
        else if (rightPlayer.Current_action == Action_state.PUSHING && leftPlayer.Current_action == Action_state.BLOCKING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.PushDodge));
        }

        //shove push
        else if (leftPlayer.Current_action == Action_state.PUSHING && rightPlayer.Current_action == Action_state.SHOVING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.PushShove));
        }
        else if (rightPlayer.Current_action == Action_state.PUSHING && leftPlayer.Current_action == Action_state.SHOVING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.PushShove));
        }

        //shove shove 
        else if (rightPlayer.Current_action == Action_state.SHOVING && leftPlayer.Current_action == Action_state.SHOVING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.ShoveShove));
        }
        //push push
        else if (rightPlayer.Current_action == Action_state.PUSHING && leftPlayer.Current_action == Action_state.PUSHING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.PushPush));
        }
        //block block
        else if (rightPlayer.Current_action == Action_state.BLOCKING && leftPlayer.Current_action == Action_state.BLOCKING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.BlockBlock));
        }
        //dodge dodge
        else if (rightPlayer.Current_action == Action_state.DODGING && leftPlayer.Current_action == Action_state.DODGING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.DodgeDodge));
        }


        //shove idle
        else if (leftPlayer.Current_action == Action_state.SHOVING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.ShoveContact));
        }
        else if (rightPlayer.Current_action == Action_state.SHOVING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.ShoveContact));
        }

        //push idle
        else if (leftPlayer.Current_action == Action_state.PUSHING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.PushContact));
        }
        else if (rightPlayer.Current_action == Action_state.PUSHING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.PushContact));
        }

        //dodge idle
        else if (leftPlayer.Current_action == Action_state.DODGING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.Dodge));
        }
        else if (rightPlayer.Current_action == Action_state.DODGING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.Dodge));
        }
        //block idle
        else if (leftPlayer.Current_action == Action_state.BLOCKING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.Block));
        }
        else if (rightPlayer.Current_action == Action_state.BLOCKING)
        {
            OnInteraction(new InteractionEventArgs(EventSource.RIGHT, EventSource.LEFT, EventType.Dodge));
        }
        //idle idle
        else if (leftPlayer.Current_action == Action_state.IDLE)
        {
            OnInteraction(new InteractionEventArgs(EventSource.LEFT, EventSource.RIGHT, EventType.Idle));
        }
 
    }

    protected virtual void OnInteraction(InteractionEventArgs e)
    {
        Interaction?.Invoke(this, e);
    }
}
