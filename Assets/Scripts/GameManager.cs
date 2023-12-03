using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	//referee
	private Referee referee;
	//sounds
	[SerializeField]
	private AudioHandler audioManager;
	//ui
	[SerializeField]
	private View view;
	//logic
	private GameLogic logic;


	[SerializeField]
	private GameConfig config;


	private static GameManager _instance;

	[SerializeField]
	private InputController _inputController;

	private HumanController humanController;

	private Player leftPlayer;

	private Player rightPlayer;

	private IActionProvider leftController;

	private IActionProvider rightController;

	private Environment environment;

	public static GameManager Instance => _instance;

	private long fixedFrameCount;

	[SerializeField]
	private int frameInterval = 5;

	[SerializeField]
	private bool debugWith2Players;

	public bool IsStarted { get; private set; }

	public bool IsPaused { get; private set; }

	void Awake() {
		if( _instance != null )
			Destroy(_instance.gameObject);
		_instance = this;
		Initialize();
	}

	void Update() {
		leftPlayer.Update();
		rightPlayer.Update();
	}

	private bool firstTick;

	private void FixedUpdate() {
		if( IsStarted && !IsPaused ) {
			fixedFrameCount++;
			if( fixedFrameCount % frameInterval == 0 ) {
				if( firstTick ) {
					environment.Start();
					leftPlayer.Start();
					rightPlayer.Start();
					firstTick = false;
				}
				referee.Tick();
			}
		}
	}

	public void Play() {
		IsStarted = true;
		IsPaused = false;
	}

	private void Initialize() {
		fixedFrameCount = 0;
		firstTick = true;
		//create player and environment from the configs

		Debug.Log($"game in progress is null? {GameInProgress.Instance == null}");
		leftPlayer = new Player(GetLeftPlayerConfig(), EventSource.LEFT);
		rightPlayer = new Player(GetRightPlayerConfig(), EventSource.RIGHT);
		leftController = GetLeftController();
		rightController = GetRightController();
		leftPlayer.Bind(leftController);
		rightPlayer.Bind(rightController);

		environment = new Environment(
			config.environmentConfig,
			leftPlayer.Config.health,
			rightPlayer.Config.health);

		referee = new Referee(leftPlayer, rightPlayer, environment);
		logic = new GameLogic(leftPlayer, rightPlayer, environment);
		logic.Bind(referee, leftPlayer, rightPlayer);

		if( view != null )
			view.Bind(environment, leftPlayer, rightPlayer);
		if( audioManager != null )
			audioManager.Bind(referee, leftPlayer, rightPlayer, environment, config.specialEffectConfig);

		referee.RefereeEvent += Referee_OnInteraction;


	}

	private IActionProvider GetLeftController() {
		var input = new InputController(0);
		GameInProgress.Instance.LeftInput = input;
		return new HumanController(input);
	}

	private IActionProvider GetRightController() {
		InputController input = null;
		IActionProvider actionProvider;
		var gameInProgress = GameInProgress.Instance;
		if( debugWith2Players ) {
			input = new(1);
			actionProvider = new HumanController(input);
		}
		else if( gameInProgress == null || gameInProgress.PlayerCount < 2 ) {
			actionProvider = new AIController(rightPlayer, leftPlayer, config.aIConfig);
		}
		else {
			input = new InputController(1);
			actionProvider = new HumanController(input);
		}
		gameInProgress.RightInput = input;
		return actionProvider;
	}

	private PlayerConfig GetLeftPlayerConfig() {
		var gameInProgress = GameInProgress.Instance;
		return gameInProgress == null
			? config.leftPlayerConfig
			: gameInProgress.LeftPlayer;
	}

	private PlayerConfig GetRightPlayerConfig() {
		var gameInProgress = GameInProgress.Instance;
		return gameInProgress == null
			? config.rightPlayerConfig
			: gameInProgress.RightPlayer;
	}

	private void Referee_OnInteraction(object sender, RefereeEventArgs e) {
		if( e.type == RefereeEventType.Win ) {
			GameInProgress.Instance.WinnerId = e.receiver == EventSource.LEFT ? 0 : 1;
			IsStarted = false;
			SceneManager.LoadScene("GameOver");
		}
	}
}