using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player
{
    public int Stamina;

    private IActionProvider input;

    private Action desiredAction;

    private ActionObject shove;
    private ActionObject push;
    private ActionObject dodge;
    private ActionObject block;

    private Queue<Action_state> actionqueue;

    public float Position { get; private set; }
    public List<Action_state> ActionList => actionqueue.ToList();

    public Player(ActionObject shove, ActionObject push, ActionObject dodge, ActionObject block)
    {
        this.shove = shove;
        this.push = push;
        this.dodge = dodge;
        this.block = block;
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
