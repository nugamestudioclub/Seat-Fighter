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
            if (stamina == 0)
            {
                Stun();
            }
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
        
        if (CurrentFrameData.state == ActionState.IDLE)
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
            && CurrentFrameData.state == ActionState.BLOCKING
            && NextFrameData.state != ActionState.BLOCKING)
        {
            ActionList.Insert(0, CurrentFrameData);
        }
        else if (desiredAction == Action.Push
            && CurrentFrameData.state == ActionState.PUSHING
            && NextFrameData.state != ActionState.PUSHING)
        {
            ActionList.Insert(0, CurrentFrameData);
        }
        if (ActionList.Count > 0)
        {
            ActionList.RemoveAt(0);
        }
        if (CurrentFrameData.state == ActionState.IDLE)
        {
            //this value should come from the player config
            Stamina += config.idleStaminaRegen;
            
        }
        else if (CurrentFrameData.state == ActionState.BLOCKING)
        {
            Stamina += config.block.holdStaminaModifier;
        }
        //reset input
        desiredAction = Action.None;
        //send on Tick event
        OnTickPlayerEvent(new PlayerTickEventArgs(playerSide, CurrentFrameData));
        return CurrentFrameData;
    }

    private void ExecuteAction(ActionConfig move)
    {
        ActionList.AddRange(move.GetFrameData());
    }

    private void Stun() {
        if (CurrentFrameData.state != ActionState.STUNNED)
        {
            ActionList.Clear();
            ExecuteAction(config.stunned);
            OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Stun, Stamina, maxStamina));
        }
        
    }

    public void Shove()
    {
        Stamina += config.shove.initialStaminaModifier;
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Shove, Stamina, maxStamina));
        ExecuteAction(config.shove);
    }

    public void Push()
    {
        Stamina += config.push.initialStaminaModifier;
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Push, Stamina, maxStamina));
        ExecuteAction(config.push);
    }
    public void Dodge()
    {
        Stamina += config.dodge.initialStaminaModifier;
        OnPlayerEvent(new PlayerEventArgs(playerSide, Action.Dodge, Stamina, maxStamina));
        ExecuteAction(config.dodge);
    }

    public void Block()
    {
        Stamina += config.block.initialStaminaModifier;
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
