using UnityEngine;
using System;

public class GameLogic {
	private GameConfig config;

	private Player leftPlayer;
	private Player rightPlayer;
	private Environment environment;

	public GameLogic(GameConfig config, Player leftPlayer, Player rightPlayer, Environment environment) {
		this.config = config;
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
			break;
		case RefereeEventType.BlockIdle:
			break;

		case RefereeEventType.DodgeDodge:
			break;
		case RefereeEventType.DodgeBlock:
			break;
		case RefereeEventType.DodgeIdle:
			break;

		case RefereeEventType.PushIdle:
			HandlePushIdle(sender, e);
			break;
		case RefereeEventType.PushBlock:
			break;
		case RefereeEventType.PushShove:
			break;
		case RefereeEventType.PushPush:
			break;
		case RefereeEventType.PushDodge:
			break;

		case RefereeEventType.ShoveIdle:
			HandleShoveIdle(sender, e);
			break;
		case RefereeEventType.ShoveShove:
			break;
		case RefereeEventType.ShoveBlock:
			break;
		case RefereeEventType.ShovePush:
			break;
		case RefereeEventType.ShoveDodge:
			break;

		case RefereeEventType.StunStun:
			break;
		case RefereeEventType.StunIdle:
			break;
		case RefereeEventType.StunBlock:
			break;
		case RefereeEventType.StunPush:
			break;
		case RefereeEventType.StunShove:
			break;
		case RefereeEventType.StunDodge:
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
	
	private void HandleOutOfBounds(object sender, RefereeEventArgs e) {
		if( e.reciever == EventSource.LEFT ) {
			environment.LeftPlayerTime--;
		}
		else {
			environment.RightPlayerTime--;
		}
	}

	private void HandlePushIdle(object sender, RefereeEventArgs e) {
		if( e.reciever == EventSource.LEFT ) {
			environment.Position -= config.rightPlayerConfig.push.positionModifier;
		}
		else {
			environment.Position += config.leftPlayerConfig.push.positionModifier;
		}
	}
	
	private void HandleShoveIdle(object sender, RefereeEventArgs e) {
		if( e.reciever == EventSource.LEFT ) {
			environment.Position -= config.rightPlayerConfig.shove.positionModifier;
		}
		else {
			environment.Position += config.leftPlayerConfig.shove.positionModifier;
		}
	}

	private void HandleWin(object sender, RefereeEventArgs e) {
		if( e.reciever == EventSource.LEFT ) {
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
			break;
		case Action.Push:
			break;
		case Action.Shove:
			break;
		}
	}

	private void HandlePlayerBlock(object sender, PlayerEventArgs e) {
		if( e.sender == EventSource.LEFT ) {
			environment.LeftPlayerTime += config.leftPlayerConfig.dodge.timeModifier;
		}
		else if( sender == rightPlayer ) {
			environment.RightPlayerTime += config.rightPlayerConfig.dodge.timeModifier;
		}
	}
}