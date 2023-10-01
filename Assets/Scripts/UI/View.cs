using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class View : MonoBehaviour {
	[SerializeField]
	private Slider leftStamina;

	[SerializeField]
	private TMPro.TMP_Text leftTimer;

	[SerializeField]
	private Slider rightStamina;

	[SerializeField]
	private TMPro.TMP_Text rightTimer;

	[SerializeField]
	private Transform positionIndicator;
	private Vector3 startingPositionIndicatorPosition;
    private Quaternion startingPositionIndicatorRotation;

	[SerializeField]
	private SpriteRenderer leftSpriteRenderer;

	[SerializeField]
	private SpriteRenderer rightSpriteRenderer;

	[SerializeField]
	private ArmViewPosition armView;

	private void Awake() {
		startingPositionIndicatorPosition = positionIndicator.position;
		startingPositionIndicatorRotation = positionIndicator.rotation;
	}

	public void Bind(Environment environment, Player leftPlayer, Player rightPlayer) {
		environment.EnvironmentChangeEvent += Environment_OnChange;
		leftPlayer.PlayerEvent += Player_OnChange;
		leftPlayer.PlayerTickEvent += Player_OnTick;
		rightPlayer.PlayerEvent += Player_OnChange;
		rightPlayer.PlayerTickEvent += Player_OnTick;
	}

	private void Environment_OnChange(object sender, EnvironmentEventArgs e) {
		if( e.type == EnvironmentEventType.PosistionChange ) {
			//move players
			SetPosition(e.value/(float)e.maxValue);

		}
		else {
			SetPlayerTimer(GetViewSide(e.type), e.value);
		}
	}

	private void Player_OnChange(object sender, PlayerEventArgs e) {
		SetPlayerStamina(GetViewSide(e.sender), e.stamina/(float)e.maxStamina);
	}

	private void Player_OnTick(object sender, PlayerTickEventArgs e) {
		(e.sender switch {
			EventSource.LEFT => leftSpriteRenderer,
			EventSource.RIGHT => rightSpriteRenderer,
			_ => throw new InvalidOperationException()
		}).sprite = e.actionFrameData.sprite;
	}

	private ViewSide GetViewSide(EventSource source) {
		return source switch {
			EventSource.LEFT => ViewSide.Left,
			EventSource.RIGHT => ViewSide.Right,
			_ => ViewSide.None
		};
	}

	private ViewSide GetViewSide(EnvironmentEventType type) {
		return type switch {
			EnvironmentEventType.LeftPlayerTimerUpdated => ViewSide.Left,
			EnvironmentEventType.RightPlayerTimerUpdated => ViewSide.Right,
			_ => ViewSide.None
		};
	}

	private void SetPlayerTimer(ViewSide side, int value) {
		(side switch {
			ViewSide.Left => leftTimer,
			ViewSide.Right => rightTimer,
			_ => throw new ArgumentOutOfRangeException(nameof(side))
		}).text = value.ToString();
	}

	private void SetPlayerStamina(ViewSide side, float value) {
		(side switch {
			ViewSide.Left => leftStamina,
			ViewSide.Right => rightStamina,
			_ => throw new ArgumentOutOfRangeException(nameof(side))
		}).value = value;
	}

	private void SetPosition(float value) {
		armView.UpdateTransform(value);

        positionIndicator.position = startingPositionIndicatorPosition;
        positionIndicator.rotation = startingPositionIndicatorRotation;
		
		float valueConstrained = Math.Max(0, Math.Min(value, 1));
        positionIndicator.Rotate(0, 0, (valueConstrained * 180) - 90);
	}
}