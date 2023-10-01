using System;
using UnityEngine;
using UnityEngine.UI;

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

	public void Bind(Environment environment, Player leftPlayer, Player rightPlayer) {
		
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