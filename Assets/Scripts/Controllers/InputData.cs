using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputData {
	private readonly ButtonState[] _buttonState;

	public Vector2 Direction { get; set; }

	public InputData() {
		_buttonState = new ButtonState[Enum.GetNames(typeof(Button)).Length];
		for( int i = 0; i < _buttonState.Length; ++i )
			_buttonState[i] = new();
	}

	private InputData(IList<ButtonState> buttonState) {
		_buttonState = buttonState.ToArray();
	}

	public ButtonState GetButtonState(Button button) {
		return _buttonState[(int)button];
	}

	public void SetButtonState(Button button, ButtonState value) {
		_buttonState[(int)button] = value;
	}

	public InputData Clone() {
		return new InputData(_buttonState);
	}
}