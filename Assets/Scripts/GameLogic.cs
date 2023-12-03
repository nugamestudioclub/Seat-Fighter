using UnityEngine;
using System;

public class GameLogic {
	private readonly Player leftPlayer;
	private readonly Player rightPlayer;
	private readonly Environment environment;

	public GameLogic(Player leftPlayer, Player rightPlayer, Environment environment) {
		this.leftPlayer = leftPlayer;
		this.rightPlayer = rightPlayer;
		this.environment = environment;
	}

	public void Bind(Referee referee, Player leftPlayer, Player rightPlayer) {
		referee.RefereeEvent += Referee_OnInteraction;
		leftPlayer.PlayerEvent += Player_OnChanged;
		rightPlayer.PlayerEvent += Player_OnChanged;
	}

	private void Referee_OnInteraction(object sender, RefereeEventArgs e) {
		switch( e.type ) {
		case RefereeEventType.IdleIdle:
			break;

		case RefereeEventType.BlockBlock:
			break;
		case RefereeEventType.BlockDodge:
			HandleDodgeMiss(sender, e);
			break;
		case RefereeEventType.BlockIdle:
			break;

		case RefereeEventType.DodgeDodge:
			HandleDodgeMiss(sender, e);
			break;
		case RefereeEventType.DodgeBlock:
			HandleDodgeMiss(sender, e);
			break;
		case RefereeEventType.DodgeIdle:
			HandleDodgeMiss(sender, e);
			break;

		case RefereeEventType.PushIdle:
			HandleStandardInteraction(sender, e);
			break;
		case RefereeEventType.PushBlock:
			// half the value of pushidle rounded up
			HandleScaledInteraction(sender, e, 0.5f, 1);
			break;
		case RefereeEventType.PushShove:
			HandleScaledInteraction(sender, e, 1, 2);
			break;
		case RefereeEventType.PushPush:
			HandleStandardInteraction(sender, e);
			break;
		case RefereeEventType.PushDodge:
			HandleDodgeMiss(sender, e);
			break;

		case RefereeEventType.ShoveIdle:
			HandleStandardInteraction(sender, e);
			break;
		case RefereeEventType.ShoveShove:
			HandleStandardInteraction(sender, e);
			break;
		case RefereeEventType.ShoveBlock:
			HandleScaledInteraction(sender, e, 0.25f, 1);
			break;
		case RefereeEventType.ShovePush:
			HandleScaledInteraction(sender, e, 2.0f, 1);
			break;
		case RefereeEventType.ShoveDodge:
			HandleDodgeMiss(sender, e);
			break;

		case RefereeEventType.StunStun:
			break;
		case RefereeEventType.StunIdle:
			break;
		case RefereeEventType.StunBlock:
			break;
		case RefereeEventType.StunPush:
			HandleStandardInteraction(sender, e);
			break;
		case RefereeEventType.StunShove:
			HandleStandardInteraction(sender, e);
			break;
		case RefereeEventType.StunDodge:
			HandleDodgeMiss(sender, e);
			break;

		case RefereeEventType.StaminaRefresh:
			break;
		case RefereeEventType.OutOfBounds:
			HandleOutOfBounds(sender, e);
			break;
		case RefereeEventType.Win:
			HandleWin(sender, e);
			break;
		}
	}


	private void HandleDodgeMiss(object sender, RefereeEventArgs e) {
		if( e.receiver == EventSource.LEFT ) {
			leftPlayer.Stamina += leftPlayer.Config
				.dodge
				.staminaSelfModifer
				.Find(s => s.action == ActionState.IDLE)
				.modifier;
		}
		else {
			rightPlayer.Stamina += rightPlayer.Config
				.dodge
				.staminaSelfModifer
				.Find(s => s.action == ActionState.IDLE)
				.modifier;
		}
	}



	private void SetEnvironmentPosition(int value, EventSource source) {
		environment.Position += source == EventSource.RIGHT ? value : -value;
	}

	private void HandleOutOfBounds(object sender, RefereeEventArgs e) {
		if( e.receiver == EventSource.LEFT ) {
			environment.LeftPlayerTime--;
		}
		else {
			environment.RightPlayerTime--;
		}
	}

	private void HandleScaledInteraction(object sender, RefereeEventArgs e, float senderScale, float recieverScale) {
		(Action senderAction, Action recieverAction) = GetActions(e);
		(ActionConfig senderActionConfig, ActionConfig recieverActionConfig) = GetActionConfigs(e.sender, senderAction, recieverAction);

		if( e.sender == EventSource.LEFT ) {
			environment.Position += (int)Math.Ceiling(senderActionConfig.positionModifier * senderScale);
			environment.Position -= (int)Math.Ceiling(recieverActionConfig.positionModifier * recieverScale);
		}
		else {
			environment.Position -= (int)Math.Ceiling(senderActionConfig.positionModifier * senderScale);
			environment.Position += (int)Math.Ceiling(recieverActionConfig.positionModifier * recieverScale);
		}
	}
	private void HandleStandardInteraction(object sender, RefereeEventArgs e) {
		(Action senderAction, Action recieverAction) = GetActions(e);
		(ActionConfig senderActionConfig, ActionConfig recieverActionConfig) = GetActionConfigs(e.sender, senderAction, recieverAction);

		if( e.sender == EventSource.LEFT ) {
			environment.Position += senderActionConfig.positionModifier;
			environment.Position -= recieverActionConfig.positionModifier;
		}
		else {
			environment.Position -= senderActionConfig.positionModifier;
			environment.Position += recieverActionConfig.positionModifier;
		}
	}

	private (Action senderAction, Action recieverAction) GetActions(RefereeEventArgs e) {
		return (e.type switch {
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

	private (ActionConfig senderActionConfig, ActionConfig recieverActionConfig) GetActionConfigs(EventSource senderSource, Action senderAction, Action reciverAction) {
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
            _ => throw new Exception("Not all action cofigs are covered")
		});

		var recieverActionConfig = (reciverAction switch
		{
			Action.Block => reciever.block,
			Action.Shove => reciever.shove,
			Action.Push => reciever.push,
			Action.Stun => reciever.stunned,
			Action.Dodge => reciever.dodge,
			Action.None => reciever.block, // maybe have a idle config later?
            _ => throw new Exception("Not all action cofigs are covered")
		});

		return (senderActionConfig, recieverActionConfig);
	}

	private void HandleWin(object sender, RefereeEventArgs e) {
		if( e.receiver == EventSource.LEFT ) {
			//DISPLAY PLAYER 1 (JORDAN) WIN
			Debug.Log("PLAYER 1 WIN");
		}
		else {
			//DISPLAY PLAYER 2 (GORDON) WIN
			Debug.Log("PLAYER 2 WIN");
		}
	}

	private void Player_OnChanged(object sender, PlayerEventArgs e) {
		switch( e.action ) {
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

	private void HandlePlayerDodge(object sender, PlayerEventArgs e) {
		if( e.sender == EventSource.LEFT ) {
			environment.LeftPlayerTime += leftPlayer.Config.dodge.timeModifier;
		}
		else if( sender == rightPlayer ) {
			environment.RightPlayerTime += rightPlayer.Config.dodge.timeModifier;
		}
	}
}