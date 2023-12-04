using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;

public class GameLogic
{
    private readonly Player leftPlayer;
    private readonly Player rightPlayer;
    private readonly Environment environment;
    private static readonly Dictionary<Action, ActionConfig> defaultActions = new();

    public GameLogic(Player leftPlayer, Player rightPlayer, Environment environment, IEnumerable<ActionConfig> defaultActions)
    {
        this.leftPlayer = leftPlayer;
        this.rightPlayer = rightPlayer;
        this.environment = environment;

        foreach (var action in defaultActions)
        {
            GameLogic.defaultActions[action.Type] = action;
        }
    }

    public void Bind(Referee referee, Player leftPlayer, Player rightPlayer)
    {
        referee.RefereeEvent += Referee_OnInteraction;
        leftPlayer.PlayerEvent += Player_OnChanged;
        rightPlayer.PlayerEvent += Player_OnChanged;
    }

    private void Referee_OnInteraction(object sender, RefereeEventArgs e)
    {
        switch (e.type)
        {
            case RefereeEventType.StaminaRefresh:
                break;
            case RefereeEventType.OutOfBounds:
                HandleOutOfBounds(sender, e);
                break;
            case RefereeEventType.Win:
                HandleWin(sender, e);
                break;

            default:
                HandleInteraction(sender, e);
                break;
        }
    }

    private static ActionConfig
        GetActionConfig(ActionState state, PlayerConfig playerConfig)
    {
        return state switch
        {
            ActionState.DODGING => playerConfig.dodge,
            ActionState.PUSHING => playerConfig.push,
            ActionState.BLOCKING => playerConfig.block,
            ActionState.SHOVING => playerConfig.shove,
            _ => null,
        };
    }

    private static (int selfMod, int enemyMod) GetStaminaModifiers(ActionConfig config, ActionState enemyState)
    {
        int selfFoundIndex = config.staminaSelfModifer.FindIndex(s => s.action == enemyState);
        int selfMod = selfFoundIndex >= 0 ? config.staminaSelfModifer[selfFoundIndex].modifier : 0;

        int enemyFoundIndex = config.staminaEnemyModifer.FindIndex(s => s.action == enemyState);
        int enemyMod = enemyFoundIndex >= 0 ? config.staminaEnemyModifer[enemyFoundIndex].modifier : 0;

        return (selfMod, enemyMod);
    }

    private static float GetPositionMultiplier(ActionConfig config, ActionState enemyState)
    {
        int foundIndex = config.PositionMultipliers.FindIndex(s => s.action == enemyState);
        return foundIndex >= 0
        || (defaultActions.TryGetValue(config.Type, out config)
            && (foundIndex = config.PositionMultipliers.FindIndex(s => s.action == enemyState)) >= 0)
        ? config.PositionMultipliers[foundIndex].multiplier
        : 1;
    }

    private void HandleOutOfBounds(object sender, RefereeEventArgs e)
    {
        if (e.receiver == EventSource.LEFT)
        {
            environment.LeftPlayerTime--;
        }
        else
        {
            environment.RightPlayerTime--;
        }
    }

    private void HandleInteraction(object sender, RefereeEventArgs e)
    {
        (Action senderAction, Action recieverAction) = GetActions(e);
        (ActionConfig senderActionConfig, ActionConfig recieverActionConfig) = GetActionConfigs(e.sender, senderAction, recieverAction);

        ActionState leftPlayerState = leftPlayer.CurrentFrameData.state;
        ActionState rightPlayerState = rightPlayer.CurrentFrameData.state;

        if (e.sender == EventSource.LEFT)
        {
            var leftPlayerPositionMult = GetPositionMultiplier(senderActionConfig, rightPlayerState);
            var rightPlayerPositionMult = GetPositionMultiplier(recieverActionConfig, leftPlayerState);
            environment.Position += (int)Math.Ceiling(senderActionConfig.positionModifier * leftPlayerPositionMult);
            environment.Position -= (int)Math.Ceiling(recieverActionConfig.positionModifier * rightPlayerPositionMult);

            var (leftPlayerStamina, rightPlayerStamina) = GetStaminaModifiers(senderActionConfig, rightPlayerState);

            leftPlayer.Stamina += leftPlayerStamina;
            rightPlayer.Stamina += rightPlayerStamina;
        }
        else
        {
            var leftPlayerPositionMult = GetPositionMultiplier(recieverActionConfig, rightPlayerState);
            var rightPlayerPositionMult = GetPositionMultiplier(senderActionConfig, leftPlayerState);
            environment.Position -= (int)Math.Ceiling(senderActionConfig.positionModifier * leftPlayerPositionMult);
            environment.Position += (int)Math.Ceiling(recieverActionConfig.positionModifier * rightPlayerPositionMult);

            var (rightPlayerStamina, leftPlayerStamina) = GetStaminaModifiers(recieverActionConfig, leftPlayerState);

            leftPlayer.Stamina += leftPlayerStamina;
            rightPlayer.Stamina += rightPlayerStamina;
        }
    }

    private static (Action senderAction, Action recieverAction) GetActions(RefereeEventArgs e)
    {
        return (e.type switch
        {
            RefereeEventType.IdleIdle => (Action.None, Action.None),
            RefereeEventType.BlockBlock => (Action.Block, Action.Block),
            RefereeEventType.BlockDodge => (Action.Block, Action.Dodge),
            RefereeEventType.BlockIdle => (Action.Block, Action.None),
            RefereeEventType.DodgeDodge => (Action.Dodge, Action.Dodge),
            RefereeEventType.DodgeBlock => (Action.Dodge, Action.Block),
            RefereeEventType.DodgeIdle => (Action.Dodge, Action.None),
            RefereeEventType.PushIdle => (Action.Push, Action.None),
            RefereeEventType.PushBlock => (Action.Push, Action.Block),
            RefereeEventType.PushShove => (Action.Push, Action.Shove),
            RefereeEventType.PushPush => (Action.Push, Action.Push),
            RefereeEventType.PushDodge => (Action.Push, Action.Dodge),
            RefereeEventType.ShoveIdle => (Action.Shove, Action.None),
            RefereeEventType.ShoveShove => (Action.Shove, Action.Shove),
            RefereeEventType.ShoveBlock => (Action.Shove, Action.Block),
            RefereeEventType.ShovePush => (Action.Shove, Action.Push),
            RefereeEventType.ShoveDodge => (Action.Shove, Action.Dodge),
            RefereeEventType.StunStun => (Action.Stun, Action.Stun),
            RefereeEventType.StunIdle => (Action.Stun, Action.None),
            RefereeEventType.StunBlock => (Action.Stun, Action.Block),
            RefereeEventType.StunPush => (Action.Stun, Action.Push),
            RefereeEventType.StunShove => (Action.Stun, Action.Shove),
            RefereeEventType.StunDodge => (Action.Stun, Action.Dodge),
            _ => (Action.None, Action.None)
        });
    }

    private (ActionConfig senderActionConfig, ActionConfig recieverActionConfig) GetActionConfigs(EventSource senderSource, Action senderAction, Action reciverAction)
    {
        (PlayerConfig sender, PlayerConfig reciever) = senderSource == EventSource.LEFT ?
            (leftPlayer.Config, rightPlayer.Config) :
            (rightPlayer.Config, leftPlayer.Config);

        var senderActionConfig = (senderAction switch
        {
            Action.Block => sender.block,
            Action.Shove => sender.shove,
            Action.Push => sender.push,
            Action.Stun => sender.stunned,
            Action.Dodge => sender.dodge,
            Action.None => sender.block, // maybe have a idle config later?
            _ => throw new Exception("Not all action configs are covered")
        });

        var recieverActionConfig = (reciverAction switch
        {
            Action.Block => reciever.block,
            Action.Shove => reciever.shove,
            Action.Push => reciever.push,
            Action.Stun => reciever.stunned,
            Action.Dodge => reciever.dodge,
            Action.None => reciever.block, // maybe have a idle config later?
            _ => throw new Exception("Not all action configs are covered")
        });

        return (senderActionConfig, recieverActionConfig);
    }

    private void HandleWin(object sender, RefereeEventArgs e)
    {
        if (e.receiver == EventSource.LEFT)
        {
            //DISPLAY PLAYER 1 (JORDAN) WIN
            Debug.Log("PLAYER 1 WIN");
        }
        else
        {
            //DISPLAY PLAYER 2 (GORDON) WIN
            Debug.Log("PLAYER 2 WIN");
        }
    }

    private void Player_OnChanged(object sender, PlayerEventArgs e)
    {
        switch (e.action)
        {
            case Action.None:
                break;
            case Action.Block:
                break;
            case Action.Dodge:
                HandlePlayerDodge(sender, e);
                break;
            case Action.Push:
                break;
            case Action.Shove:
                break;
        }
    }

    private void HandlePlayerDodge(object sender, PlayerEventArgs e)
    {
        if (e.sender == EventSource.LEFT)
        {
            environment.LeftPlayerTime += leftPlayer.Config.dodge.timeModifier;
        }
        else if (sender == rightPlayer)
        {
            environment.RightPlayerTime += rightPlayer.Config.dodge.timeModifier;
        }
    }
}