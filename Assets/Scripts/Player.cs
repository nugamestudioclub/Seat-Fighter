using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private ActionObject shove;
    private ActionObject push;
    private ActionObject dodge;
    private ActionObject block;

    public float Position { get; private set; }
    private Queue<Action_state> actionqueue;
    public Action_state Current_action
    {
        get
        {
            if (actionqueue.Count > 0)
            {
                return actionqueue.Peek();
            }
            else
            {
                return Action_state.IDLE;
            }
        }
    }

    public Player(ActionObject shove, ActionObject push, ActionObject dodge, ActionObject block)
    {
        this.shove = shove;
        this.push = push;
        this.dodge = dodge;
        this.block = block;

    }

    public Player(Dictionary<Action, ActionObject> dict)
    {
        this.shove = dict[Action.Shove];
        this.push = dict[Action.Push];
        this.dodge = dict[Action.Dodge];
        this.block = dict[Action.Block];
    }

    public Action_state Tick()
    {
        if(actionqueue.Count > 0)
        {
            actionqueue.Dequeue();
        }

        return Current_action;
    }

    private void ExecuteAction(ActionObject move)
    {
        move.States.ForEach(state_duration =>
        {
            for (int i = 0; i < state_duration.duration; i++)
            {
                actionqueue.Enqueue(state_duration.state);
            }

        }
       );
    }


    public void Shove()
    {
        ExecuteAction(shove);
    }

    public void Push() { 
        ExecuteAction(push);
    }
    public void Dodge() {
        ExecuteAction(dodge);
    }

    public void Block() {
        ExecuteAction(block);
    }



}
