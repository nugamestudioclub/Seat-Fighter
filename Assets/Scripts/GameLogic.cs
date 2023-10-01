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

	public void Bind(Referee referee) {
		referee.RefereeEvent += Referee_OnInteraction;
	}

	private void Referee_OnInteraction(object sender, RefereeEventArgs e) {
		switch( e.type ) {
		case RefereeEventType.IdleIdle:
			break;

		case RefereeEventType.BlockBlock:
			break;
		case RefereeEventType.BlockDodge:
                //normal miss dodge pentalty
                break;
		case RefereeEventType.BlockIdle:
			break;

		case RefereeEventType.DodgeDodge:
                //normal miss dodge pentalty
                break;
		case RefereeEventType.DodgeBlock:
                //normal miss dodge pentalty
                break;
		case RefereeEventType.DodgeIdle:
                //normal miss dodge pentalty
                break;

		case RefereeEventType.PushIdle:
			HandlePushIdle(sender, e);
			break;
		case RefereeEventType.PushBlock:
			// half the value of pushidle rounded up
			break;
		case RefereeEventType.PushShove:
			// normal interaction +-
			break;
		case RefereeEventType.PushPush:
             // normal interaction +-
             break;
		case RefereeEventType.PushDodge:
			// do not get moved by push
			break;

		case RefereeEventType.ShoveIdle:
			HandleShoveIdle(sender, e);
			break;
		case RefereeEventType.ShoveShove:
                // normal interaction +-
                break;
		case RefereeEventType.ShoveBlock:
			// shove distnace /4 rounended up
			break;
		case RefereeEventType.ShovePush:
                // normal interaction +-
                break;
		case RefereeEventType.ShoveDodge:
				//caluclate wiff dont dot get hit
			break;

		case RefereeEventType.StunStun:
			break;
		case RefereeEventType.StunIdle:
			break;
		case RefereeEventType.StunBlock:
			break;
		case RefereeEventType.StunPush:
				//normal push
			break;
		case RefereeEventType.StunShove:
				//normal shove
			break;
		case RefereeEventType.StunDodge:
				//normal miss dodge pentalty
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
}