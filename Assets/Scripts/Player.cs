using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class Player
{

    private int stamina;
    public int Stamina
    {
        get => stamina;
        set
        {
            stamina = value;
            OnPlayerEvent(
                new PlayerEventArgs(
                    playerSide,
                    Action.None,
                    value));
        }
    }

    private IActionProvider input;

    private Action desiredAction;

    private readonly ActionObject shove;
    private readonly ActionObject push;
    private readonly ActionObject dodge;
    private readonly ActionObject block;

    private readonly EventSource playerSide;

    private readonly Queue<Action_state> actionqueue;

    public float Position { get; private set; }
    public List<Action_state> ActionList => actionqueue.ToList();

    public event EventHandler<PlayerEventArgs> PlayerEvent;

    public Player(ActionObject shove, ActionObject push, ActionObject dodge, ActionObject block, EventSource playerSide)
    {
        this.shove = shove;
        this.push = push;
        this.dodge = dodge;
        this.block = block;
        this.playerSide = playerSide;
        actionqueue = new Queue<Action_state>();
    }
    public Player(Dictionary<Action, ActionObject> dict)
    {
        this.shove = dict[Action.Shove];
        this.push = dict[Action.Push];
        this.dodge = dict[Action.Dodge];
        this.block = dict[Action.Block];
    }

    public void Bind(IActionProvider actionProvider)
    {
        input = actionProvider;
    }

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

    public void Update()
    {
        if( desiredAction == Action.None )
            desiredAction = input.GetNextAction();
    }

    public Action_state Tick()
    {
        if (Current_action == Action_state.IDLE)
        {
            switch (desiredAction)
            {
                case Action.Block:
                    Block();
                    break;
                case Action.Dodge:
                    Dodge();
                    break;
                case Action.Push:
                    Push();
                    break;
                case Action.Shove:
                    Shove();
                    break;
            }
        }
        if(actionqueue.Count > 0)
        {
            actionqueue.Dequeue();
        }
        if( Current_action == Action_state.IDLE )
            desiredAction = Action.None;
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
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Shove, Stamina));
        ExecuteAction(shove);
    }

    public void Push() {
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Push, Stamina));
        ExecuteAction(push);
    }
    public void Dodge() {
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Dodge, Stamina));
        ExecuteAction(dodge);
    }

    public void Block() {
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Block, Stamina));
        ExecuteAction(block);
    }

    protected virtual void OnPlayerEvent(PlayerEventArgs e)
    {
        PlayerEvent?.Invoke(this, e);
    }

}
