using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	private static GameManager _instance;

	[SerializeField]
	private InputController _inputController;

	private PlayerInput _playerInput;

	public static GameManager Instance => _instance;

	void Awake() {
		if( _instance == null ) {
			_instance = this;
			DontDestroyOnLoad(gameObject);
			Initialize();
		}
		else {
			Destroy(gameObject);
		}
	}

	private int frameCount;

	void Update() {
		int currentFrameCount = Time.frameCount;
		if( currentFrameCount > frameCount ) {
			var action = _playerInput.GetNextAction();
			if( action != Action.None ) {
				Debug.Log(action);
			}
			frameCount = currentFrameCount;
		}
	}

	private void Initialize() {
		_inputController.Initialize();
		_playerInput = new(_inputController);
	}
}