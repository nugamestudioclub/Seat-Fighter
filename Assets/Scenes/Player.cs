using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private Action shove;
    private Action push;
    private Action dodge;
    private Action block;

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

    public Player(Action shove, Action push, Action dodge, Action block)
    {
        this.shove = shove;
        this.push = push;
        this.dodge = dodge;
        this.block = block;

    }
    public Action_state Tick()
    {
        if(actionqueue.Count > 0)
        {
            actionqueue.Dequeue();
        }

        return Current_action;
    }

    private void ExecuteAction(Action move)
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
