using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	//referee
	private Referee referee;
	//sounds

	//ui

	//logic

	[SerializeField]
	private PlayerController leftPlayerController;
    [SerializeField]
    private PlayerController rightPlayerController;
	[SerializeField]
	private Environment environment;



    private static GameManager _instance;

	[SerializeField]
	private InputController _playerInput;

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

	private readonly IList<int> buttonValues = Buttons.GetValues();
	private readonly IList<string> buttonNames = Buttons.GetNames();

	void Update() {
		for( int i = 0; i < buttonValues.Count; ++i ) {
			var button = (Button)i;
			if( _playerInput.IsDown(button) )
				Debug.Log($"{buttonNames[i]} is down");
			else if( _playerInput.IsPressed(button) )
				Debug.Log($"{buttonNames[i]} is pressed");
			else if( _playerInput.IsUp(button) )
				Debug.Log($"{buttonNames[i]} is up");
		}
	}

	private void Initialize() {
		_playerInput.Initialize();

		//referee = new Referee(leftPlayerController.);
	}
}