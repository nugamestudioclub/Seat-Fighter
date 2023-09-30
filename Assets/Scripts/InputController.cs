using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour {
	[SerializeField]
	private int _id;

	private InputActionMap _actionMap;

	private InputData _inputData;

	public void Initialize() {
		var devices = FindDevices(_id);
		var data = new InputData();
		_actionMap = MapInput("Player_" + _id.ToString(), devices, data);
		_inputData = data;
	}

	public static void BindAxis(
		string name,
		InputActionMap actionMap,
		Action<float> setter,
		IEnumerable<string> positivePaths,
		IEnumerable<string> negativePaths
	) {
		var action = actionMap.AddAction(name);
		var binding = action.AddCompositeBinding("Axis");

		foreach( var positivePath in positivePaths )
			binding.With("Positive", positivePath);
		foreach( var negativePath in negativePaths )
			binding.With("Negative", negativePath);

		action.performed += context => setter(context.ReadValue<float>());
	}

	public static void BindButton(
		string name, InputActionMap actionMap, Action<bool> setter,
		params string[] paths
	) {
		BindButton(name, actionMap, setter, (IEnumerable<string>)paths);
	}

	public static void BindButton(
		string name,
		InputActionMap actionMap,
		Action<bool> setter,
		IEnumerable<string> paths
	) {
		var action = actionMap.AddAction(name);

		foreach( var path in paths )
			action.AddBinding(path);

		action.performed += _ => setter(true);
		action.canceled += _ => setter(false);
	}

	public static void BindVector2(
		string name,
		InputActionMap actionMap,
		Action<Vector2> setter,
		IEnumerable<string> upPaths,
		IEnumerable<string> downPaths,
		IEnumerable<string> leftPaths,
		IEnumerable<string> rightPaths
	) {
		var action = actionMap.AddAction(name);
		var binding = action.AddCompositeBinding("2DVector");

		foreach( var upPath in upPaths )
			binding.With("Up", upPath);
		foreach( var downPath in downPaths )
			binding.With("Down", downPath);
		foreach( var leftPath in leftPaths )
			binding.With("Left", leftPath);
		foreach( var rightPath in rightPaths )
			binding.With("Right", rightPath);

		action.performed += context => setter(context.ReadValue<Vector2>());
		action.canceled += context => setter(Vector2.zero);
	}

	private static IList<InputDevice> FindDevices(int playerId) {
		var devices = new List<InputDevice>();

		if( playerId < Gamepad.all.Count )
			devices.Add(Gamepad.all[playerId]);
		else
			Debug.Log($"Gamepad {playerId} is not connected");

		if( playerId == 0 )
			devices.Add(Keyboard.current);
		if( playerId == 0 )
			devices.Add(Mouse.current);

		return devices;
	}

	public bool IsDown(Button button) => _inputData.GetButtonState(button).IsDown;

	public bool IsPressed(Button button) => _inputData.GetButtonState(button).IsPressed;

	public bool IsUp(Button button) => _inputData.GetButtonState(button).IsUp;

	private static InputActionMap MapInput(string name, IEnumerable<InputDevice> devices, InputData data) {
		var actionMap = new InputActionMap(name);

		BindButton("Start", actionMap, v => data.SetButtonState(Button.Start, v), "<Keyboard>/enter", "<Gamepad>/leftShoulder");
		BindButton("Cancel", actionMap, v => data.SetButtonState(Button.Cancel, v), "<Keyboard>/escape", "<Gamepad>/rightShoulder");
		BindButton("Block", actionMap, v => data.SetButtonState(Button.Block, v), "<Keyboard>/k", "<Gamepad>/buttonSouth");
		BindButton("Dodge", actionMap, v => data.SetButtonState(Button.Dodge, v), "<Keyboard>/j", "<Gamepad>/buttonWest");
		BindButton("Push", actionMap, v => data.SetButtonState(Button.Push, v), "<Keyboard>/i", "<Gamepad>/buttonNorth");
		BindButton("Shove", actionMap, v => data.SetButtonState(Button.Push, v), "<Keyboard>/l", "<Gamepad>/buttonEast");

		actionMap.devices = devices as InputDevice[] ?? devices.ToArray();
		actionMap.Enable();

		return actionMap;
	}
}