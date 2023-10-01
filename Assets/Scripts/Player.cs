using System;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    private readonly int maxStamina;
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
                    value,
                    maxStamina));
        }
    }

    private IActionProvider input;

    private Action desiredAction;

    private readonly ActionConfig shove;
    private readonly ActionConfig push;
    private readonly ActionConfig dodge;
    private readonly ActionConfig block;

    private readonly EventSource playerSide;

    private readonly Queue<ActionState> actionqueue;

    public float Position { get; private set; }
    public List<ActionState> ActionList => actionqueue.ToList();

    public event EventHandler<PlayerEventArgs> PlayerEvent;

    public Player(ActionConfig shove, ActionConfig push, ActionConfig dodge, ActionConfig block, int maxStamina, EventSource playerSide)
    {
        this.shove = shove;
        this.push = push;
        this.dodge = dodge;
        this.block = block;
        this.playerSide = playerSide;
        this.maxStamina = maxStamina;
        Stamina = maxStamina;
        actionqueue = new Queue<ActionState>();
    }
    public Player(Dictionary<Action, ActionConfig> dict)
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

    public ActionState Current_action
    {
        get
        {
            if (actionqueue.Count > 0)
            {
                return actionqueue.Peek();
            }
            else
            {
                return ActionState.IDLE;
            }
        }
    }

    public void Update()
    {
        if( desiredAction == Action.None )
            desiredAction = input.GetNextAction();
    }

    public ActionState Tick()
    {
        if (Current_action == ActionState.IDLE)
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
        if( Current_action == ActionState.IDLE )
            desiredAction = Action.None;
        return Current_action;
    }

    private void ExecuteAction(ActionConfig move)
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
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Shove, Stamina, maxStamina));
        ExecuteAction(shove);
    }

    public void Push() {
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Push, Stamina, maxStamina));
        ExecuteAction(push);
    }
    public void Dodge() {
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Dodge, Stamina, maxStamina));
        ExecuteAction(dodge);
    }

    public void Block() {
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Block, Stamina, maxStamina));
        ExecuteAction(block);
    }

    protected virtual void OnPlayerEvent(PlayerEventArgs e)
    {
        PlayerEvent?.Invoke(this, e);
    }

}
