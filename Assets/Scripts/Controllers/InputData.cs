using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InputData {
	private readonly ButtonState[] _buttonStates;
	public List<ButtonState> ButtonStates { get { return new (_buttonStates); } }
    public Vector2 Direction { get; set; }

	public InputData() {
		_buttonStates = new ButtonState[Enum.GetNames(typeof(Button)).Length];
		for( int i = 0; i < _buttonStates.Length; ++i )
			_buttonStates[i] = new();
	}
    public InputData(InputData inputData)
	{
		_buttonStates = inputData.ButtonStates.ToArray();
    }


    private InputData(IList<ButtonState> buttonState) {
		_buttonStates = buttonState.ToArray();
	}

	public ButtonState GetButtonState(Button button) {
		return _buttonStates[(int)button];
	}

	public void SetButtonState(Button button, ButtonState value) {
		_buttonStates[(int)button] = value;
	}

	public InputData Clone() {
		return new InputData(_buttonStates);
	}
}