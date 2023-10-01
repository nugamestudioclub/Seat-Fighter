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
            stamina = Math.Max(0, Math.Min(value, maxStamina));
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

    private readonly PlayerConfig config;

    private readonly EventSource playerSide;

    private readonly List<ActionFrameData> actionqueue;

    public float Position { get; private set; }
    public List<ActionFrameData> ActionList => actionqueue.ToList();

    public event EventHandler<PlayerEventArgs> PlayerEvent;

    public event EventHandler<PlayerTickEventArgs> PlayerTickEvent;

    public Player(PlayerConfig config, EventSource playerSide)
    {
        this.config = config;
        this.playerSide = playerSide;
        maxStamina = config.maxStamina;
        Stamina = maxStamina;
        actionqueue = new();
    }

    public void Bind(IActionProvider actionProvider)
    {
        input = actionProvider;
    }

    public ActionFrameData CurrentActionData
    {
        get
        {
            if (actionqueue.Count > 0)
            {
                return actionqueue[0];
            }
            else
            {
                return new ActionFrameData(ActionState.IDLE, config.idleSprite);
            }
        }
    }

    public void Update()
    {
        if (desiredAction == Action.None)
            desiredAction = input.GetNextAction();
    }

    public ActionFrameData Tick()
    {
        if (CurrentActionData.state == ActionState.IDLE)
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
        if (actionqueue.Count > 0)
        {
            actionqueue.RemoveAt(0);
        }
        if (CurrentActionData.state == ActionState.IDLE)
        {
            //this value should come from the player config
            Stamina += 10;
            desiredAction = Action.None;
        }
        else if (CurrentActionData.state == ActionState.BLOCKING)
        {
            Stamina += 5;
        }
        //send on Tick event
        OnTickPlayerEvent(new PlayerTickEventArgs(playerSide, CurrentActionData));
        return CurrentActionData;
    }

    private void ExecuteAction(ActionConfig move)
    {
        actionqueue.AddRange(move.GetFrameData());
    }


    public void Shove()
    {
        Stamina += config.shove.StaminaModifier;
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Shove, Stamina, maxStamina));
        ExecuteAction(config.shove);
    }

    public void Push()
    {
        Stamina += config.push.StaminaModifier;
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Push, Stamina, maxStamina));
        ExecuteAction(config.push);
    }
    public void Dodge()
    {
        Stamina += config.dodge.StaminaModifier;
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Dodge, Stamina, maxStamina));
        ExecuteAction(config.dodge);
    }

    public void Block()
    {
        Stamina += config.block.StaminaModifier;
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Block, Stamina, maxStamina));
        ExecuteAction(config.block);
    }

    protected virtual void OnPlayerEvent(PlayerEventArgs e)
    {
        PlayerEvent?.Invoke(this, e);
    }

    protected virtual void OnTickPlayerEvent(PlayerTickEventArgs e)
    {
        PlayerTickEvent?.Invoke(this, e);
    }

}
