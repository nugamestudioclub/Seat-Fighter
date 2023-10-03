using System;
using System.Collections.Generic;

public class Player
{
    public readonly int maxStamina;
    private int stamina;
    public int Stamina
    {
        get => stamina;
        set
        {
            stamina = Math.Clamp(value, 0, maxStamina);
            OnPlayerEvent(
                new PlayerEventArgs(
                    playerSide,
                    Action.None,
                    value,
                    maxStamina));
            if (!HasLowStamina)
            {
                signaledLowStamina = false;
            }
        }
    }
    public bool HasLowStamina => Stamina < (maxStamina * 0.25f);
    private bool signaledLowStamina;

    private IActionProvider input;

    private Action desiredAction;

    private readonly PlayerConfig config;

    private readonly EventSource playerSide;

    public List<ActionFrameData> ActionList { get; private set; }

    public float Position { get; private set; }

    public event EventHandler<PlayerEventArgs> PlayerEvent;

    public event EventHandler<PlayerTickEventArgs> PlayerTickEvent;

    public event EventHandler<PlayerStateEventArgs> PlayerStateEvent;

    public Player(PlayerConfig config, EventSource playerSide)
    {
        signaledLowStamina = false;
        this.config = config;
        this.playerSide = playerSide;
        maxStamina = config.maxStamina;
        Stamina = maxStamina;
        ActionList = new();
    }

    public void Start()
    {
        Stamina = maxStamina;
    }
    public void Bind(IActionProvider actionProvider)
    {
        input = actionProvider;
    }

    public ActionFrameData CurrentFrameData { get => FrameDataAt(0); }

    public ActionFrameData NextFrameData { get => FrameDataAt(1); }

    public ActionFrameData FrameDataAt(int index)
    {
        if (ActionList.Count > index)
        {
            return ActionList[index];
        }
        else
        {
            return new ActionFrameData(ActionState.IDLE, config.idleSprite);
        }
    }

    public void Update()
    {
        if (desiredAction == Action.None)
            desiredAction = input.GetNextAction();
    }

    public ActionFrameData Tick()
    {
        ActionState currentState = CurrentFrameData.state;
        ActionState nextState = NextFrameData.state;
        
        if (currentState == ActionState.IDLE)
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
        else if (desiredAction == Action.Block
            && currentState == ActionState.BLOCKING
            && nextState != ActionState.BLOCKING)
        {
            ActionList.Insert(0, CurrentFrameData);
        }
        else if (desiredAction == Action.Push
            && currentState == ActionState.PUSHING
            && nextState != ActionState.PUSHING)
        {
            ActionList.Insert(0, CurrentFrameData);
        }

        if (Stamina <= 0)
        {
            Stun();
        }
        ActionState nowState = CurrentFrameData.state;
        if (nowState == ActionState.IDLE)
        {
            //this value should come from the player config
            Stamina += config.idleStaminaRegen;

        }
        else if (nowState != ActionState.BUSY)
        {
            HandleActionState(FindMove(currentState));
        }
        if (HasLowStamina && !signaledLowStamina)
        {
            signaledLowStamina = true;
            OnPlayerStateEvent(new PlayerStateEventArgs(playerSide, PlayerState.LowStamina));
        }
        //reset input
        desiredAction = Action.None;
        if (ActionList.Count > 0)
        {
            ActionList.RemoveAt(0);
        }
        //send on Tick event
        OnTickPlayerEvent(new PlayerTickEventArgs(playerSide, CurrentFrameData));
        return CurrentFrameData;
    }
    private ActionConfig FindMove(ActionState state)
    {
        return (state switch
        {
            ActionState.PUSHING => config.push,
            ActionState.BLOCKING => config.block,
            ActionState.SHOVING => config.shove,
            ActionState.STUNNED => config.stunned,
            ActionState.DODGING => config.dodge,
            _ => throw new ArgumentException($"Cannot handle state {state}", nameof(state))
        });
    }
    private void HandleActionState(ActionConfig move)
    {
        Stamina += move.holdStaminaModifier;
    }
    private void ExecuteAction(ActionConfig move)
    {
        Stamina += move.initialStaminaModifier;
        ActionList.AddRange(move.GetFrameData());
    }

    private void Stun()
    {
        if (CurrentFrameData.state != ActionState.STUNNED)
        {
            ActionList.Clear();            
            OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Stun, Stamina, maxStamina));
            ExecuteAction(config.stunned);
        }

    }

    public void Shove()
    {
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Shove, Stamina, maxStamina));
        ExecuteAction(config.shove);
    }

    public void Push()
    {
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Push, Stamina, maxStamina));
        ExecuteAction(config.push);
    }
    public void Dodge()
    {
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Dodge, Stamina, maxStamina));
        ExecuteAction(config.dodge);
    }

    public void Block()
    {
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

    protected virtual void OnPlayerStateEvent(PlayerStateEventArgs e)
    {
        PlayerStateEvent?.Invoke(this, e);
    }

}
