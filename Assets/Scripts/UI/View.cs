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
	private Transform position;

	[SerializeField]
	private BubbleOptionsConfig bubbleOptions;

	private Dictionary<string, BubbleConfig> bubbles;

	private void Awake() {
		bubbles = new(bubbleOptions.Options.Select(x => new KeyValuePair<string, BubbleConfig>(x.name, x.bubble)));
	}

	public void Bind(Environment environment, Player leftPlayer, Player rightPlayer) {
		environment.EnvironmentChangeEvent += Environment_OnChange;
		leftPlayer.PlayerEvent += Player_OnChange;
        rightPlayer.PlayerEvent += Player_OnChange;
    }

	private void Environment_OnChange(object sender, EnvironmentEventArgs e)
	{
		if (e.type == EnvironmentEventType.PosistionChange)
		{
			//move players
			SetPosition(e.value/(float) e.maxValue);

        }
		else
		{
            SetPlayerTimer(GetViewSide(e.type), e.value);
        }
    }

    private void Player_OnChange(object sender, PlayerEventArgs e)
    {
		SetPlayerStamina(GetViewSide(e.sender), e.stamina/(float)e.maxStamina);
    }

	private ViewSide GetViewSide(EventSource source)
	{
		return source switch
		{
			EventSource.LEFT => ViewSide.Left,
			EventSource.RIGHT => ViewSide.Right,
			_ => ViewSide.None
		};
	}

    private ViewSide GetViewSide(EnvironmentEventType type)
    {
        return type switch
        {
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
		position.Rotate(0, 0, (value * 180) - 90);
	}
}