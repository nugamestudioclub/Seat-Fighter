using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Referee
{
    private Player leftPlayer;
    private Player rightPlayer;
    private int position;

    public event EventHandler<InteractionEventArgs> Interaction;
    public Referee(Player leftPlayer, Player rightPlayer) : this(leftPlayer, rightPlayer, 50) { }
    

    public Referee(Player leftPlayer, Player rightPlayer, int startingPosition)
    {
        this.leftPlayer = leftPlayer;
        this.rightPlayer = rightPlayer;
        position = startingPosition;
    }

    public void Tick()
    {
        leftPlayer.Tick();
        rightPlayer.Tick();
        ResolveEvents(leftPlayer, rightPlayer, position);
    }

    private void ResolveEvents(Player leftPlayer, Player rightPlayer, int position)
    {
        // game logic
        if (position <= 0) //left player is oob
        {
            OnInteraction(new InteractionEventArgs(EventSource.ENVIRONEMNT, EventSource.LEFT, EventType.OutOfBounds));
        } else if (position >= 100)
        {
            OnInteraction(new InteractionEventArgs(EventSource.ENVIRONEMNT, EventSource.RIGHT, EventType.OutOfBounds));
        }


        ResolveActionEvents(leftPlayer, rightPlayer, position);



    }


    private void ResolveActionEvents(Player leftPlayer, Player rightPlayer, int position)
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
