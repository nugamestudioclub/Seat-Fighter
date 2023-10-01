using System;
using UnityEngine;

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
        Debug.Log($"Left action: {leftPlayer.CurrentActionData.state}");
        Debug.Log($"Right action: {rightPlayer.CurrentActionData.state}");
        ResolveEvents(leftPlayer, rightPlayer, environment);
    }

    private void ResolveEvents(Player leftPlayer, Player rightPlayer, Environment environment)
    {
        // game logic
        if (environment.LeftPlayerTime <=0)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.ENVIRONEMNT, EventSource.LEFT, RefereeEventType.Win));

        }
        else if (environment.RightPlayerTime <=0)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.ENVIRONEMNT, EventSource.RIGHT, RefereeEventType.Win));

        }
        else if (environment.Position <= 0) //left player is oob
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.ENVIRONEMNT, EventSource.LEFT, RefereeEventType.OutOfBounds));
        } 
        else if (environment.Position >= environment.Config.armrestWidth)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.ENVIRONEMNT, EventSource.RIGHT, RefereeEventType.OutOfBounds));
        }

        ResolveActionEvents(leftPlayer, rightPlayer);



    }


    private void ResolveActionEvents(Player leftPlayer, Player rightPlayer)
    {
        ActionState leftPlayerState = leftPlayer.CurrentActionData.state;
        ActionState rightPlayerState = rightPlayer.CurrentActionData.state;
        //blocking shove
        if (leftPlayerState == ActionState.SHOVING && rightPlayerState == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushBlock));
        } 
        else if(rightPlayerState == ActionState.SHOVING && leftPlayerState == ActionState.BLOCKING) {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushBlock));
        }

        //blocking push
        else if (leftPlayerState == ActionState.PUSHING && rightPlayerState == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushBlock));
        }
        else if (rightPlayerState == ActionState.PUSHING && leftPlayerState == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushBlock));
        }

        //dodge shove
        else if (leftPlayerState == ActionState.SHOVING && rightPlayerState == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.ShoveDodge));
        }
        else if (rightPlayerState == ActionState.SHOVING && leftPlayerState == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.ShoveDodge));
        }

        //dodge push
        else if (leftPlayerState == ActionState.PUSHING && rightPlayerState == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushDodge));
        }
        else if (rightPlayerState == ActionState.PUSHING && leftPlayerState == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushDodge));
        }

        //shove push
        else if (leftPlayerState == ActionState.PUSHING && rightPlayerState == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushShove));
        }
        else if (rightPlayerState == ActionState.PUSHING && leftPlayerState == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushShove));
        }

        //shove shove 
        else if (rightPlayerState == ActionState.SHOVING && leftPlayerState == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.ShoveShove));
        }
        //push push
        else if (rightPlayerState == ActionState.PUSHING && leftPlayerState == ActionState.PUSHING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushPush));
        }
        //block block
        else if (rightPlayerState == ActionState.BLOCKING && leftPlayerState == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.BlockBlock));
        }
        //dodge dodge
        else if (rightPlayerState == ActionState.DODGING && leftPlayerState == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.DodgeDodge));
        }


        //shove idle
        else if (leftPlayerState == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.ShoveContact));
        }
        else if (rightPlayerState == ActionState.SHOVING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.ShoveContact));
        }

        //push idle
        else if (leftPlayerState == ActionState.PUSHING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.PushContact));
        }
        else if (rightPlayerState == ActionState.PUSHING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.PushContact));
        }

        //dodge idle
        else if (leftPlayerState == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.Dodge));
        }
        else if (rightPlayer.CurrentActionData.state == ActionState.DODGING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.Dodge));
        }
        //block idle
        else if (leftPlayerState == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.Block));
        }
        else if (rightPlayerState == ActionState.BLOCKING)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.RIGHT, EventSource.LEFT, RefereeEventType.Dodge));
        }
        //idle idle
        else if (leftPlayerState == ActionState.IDLE)
        {
            OnRefereeEvent(new RefereeEventArgs(EventSource.LEFT, EventSource.RIGHT, RefereeEventType.Idle));
        }
 
    }

    protected virtual void OnRefereeEvent(RefereeEventArgs e)
    {
        RefereeEvent?.Invoke(this, e);
    }
}
