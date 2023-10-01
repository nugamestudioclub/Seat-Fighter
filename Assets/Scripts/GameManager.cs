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

	private Environment environment;

	public static GameManager Instance => _instance;

	private long fixedFrameCount;

	[SerializeField]
	private int frameInterval = 5;

	public int winnerId { get; private set; }

	public bool IsStarted { get; private set; }

	public bool IsPaused { get; private set; }

	public bool ShowOnScreenControls { get; private set; }

	public int PlayerCount { get; private set; } = 1;

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

	void Update() {
		leftPlayer.Update();
		rightPlayer.Update();
	}

	private bool firstTick;

	private void FixedUpdate() {
		if( IsStarted && !IsPaused ) {
			fixedFrameCount++;
			if( fixedFrameCount % frameInterval == 0 ) {
				if (firstTick)
				{
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
		leftPlayer = new Player(config.leftPlayerConfig, EventSource.LEFT);
		rightPlayer = new Player(config.rightPlayerConfig, EventSource.RIGHT);

		_inputController.Initialize();
		humanController = new(_inputController);
		var aiController = new AIController(rightPlayer, leftPlayer, config.aIConfig);

		leftPlayer.Bind(humanController);
		rightPlayer.Bind(aiController);

		environment = new Environment(
			config.environmentConfig,
			config.leftPlayerConfig.health,
			config.rightPlayerConfig.health);

		referee = new Referee(leftPlayer, rightPlayer, environment);
		logic = new GameLogic(config, leftPlayer, rightPlayer, environment);
		logic.Bind(referee);

		if( view != null )
			view.Bind(environment, leftPlayer, rightPlayer);
		if( audioManager != null )
			audioManager.Bind(referee, leftPlayer, rightPlayer, environment, config.specialEffectConfig);

		referee.RefereeEvent += Referee_OnInteraction;
	
    }

	private void Referee_OnInteraction(object sender, RefereeEventArgs e) {
		if( e.type == RefereeEventType.Win ) {
			winnerId = e.sender == EventSource.LEFT ? 0 : 1;
			IsStarted = false;
			view.enabled = false;
			audioManager.enabled = false;
			SceneManager.LoadScene("GameOver");
		}
	}
}