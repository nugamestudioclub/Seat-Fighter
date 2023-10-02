using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class Player
{
    public readonly int maxStamina;
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

    public List<ActionFrameData> ActionList { get; private set; }

    public float Position { get; private set; }

    public event EventHandler<PlayerEventArgs> PlayerEvent;

    public event EventHandler<PlayerTickEventArgs> PlayerTickEvent;

    public Player(PlayerConfig config, EventSource playerSide)
    {
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
        if (ActionList.Count > 0)
        {
            ActionList.RemoveAt(0);
        }
        if (currentState == ActionState.IDLE)
        {
            //this value should come from the player config
            Stamina += config.idleStaminaRegen;

        }
        else if (currentState != ActionState.BUSY)
        {
            HandleActionState(FindMove(currentState));
        }

        //reset input
        desiredAction = Action.None;
        //send on Tick event
        OnTickPlayerEvent(new PlayerTickEventArgs(playerSide, CurrentFrameData));
        if (stamina == 0)
        {
            Stun();
        }
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
            ExecuteAction(config.stunned);
            OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Stun, Stamina, maxStamina));
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

}
