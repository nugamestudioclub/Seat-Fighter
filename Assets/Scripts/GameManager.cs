using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

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

    void Awake()
    {
        if (_instance != null)
            Destroy(_instance.gameObject);
        _instance = this;
        Initialize();
    }

    void Update()
    {
        leftPlayer.Update();
        rightPlayer.Update();
    }

    private bool firstTick;

    private void FixedUpdate()
    {
        if (IsStarted && !IsPaused)
        {
            fixedFrameCount++;
            if (fixedFrameCount % frameInterval == 0)
            {
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

    public void Play()
    {
        IsStarted = true;
        IsPaused = false;
        GameInProgress.Instance.Config = config;
    }

    private void Initialize()
    {
        fixedFrameCount = 0;
        firstTick = true;
        //create player and environment from the configs
        leftPlayer = new Player(config.leftPlayerConfig, EventSource.LEFT);
        rightPlayer = new Player(config.rightPlayerConfig, EventSource.RIGHT);
        leftController = GetLeftController();
        rightController = GetRightController();
        leftPlayer.Bind(leftController);
        rightPlayer.Bind(rightController);

        environment = new Environment(
            config.environmentConfig,
            config.leftPlayerConfig.health,
            config.rightPlayerConfig.health);

        referee = new Referee(leftPlayer, rightPlayer, environment);
        logic = new GameLogic(config, leftPlayer, rightPlayer, environment);
        logic.Bind(referee, leftPlayer, rightPlayer);

        if (view != null)
            view.Bind(environment, leftPlayer, rightPlayer);
        if (audioManager != null)
            audioManager.Bind(referee, leftPlayer, rightPlayer, environment, config.specialEffectConfig);

        referee.RefereeEvent += Referee_OnInteraction;

    }

    private IActionProvider GetLeftController()
    {
        var input = new InputController(0);
        return new HumanController(input);
    }
	
	private IActionProvider GetRightController() {
        if( debugWith2Players )
            return new HumanController(new InputController(1));

        var gameInProgress = GameInProgress.Instance;

        if (gameInProgress == null || gameInProgress.PlayerCount < 2)
        {
            return new AIController(rightPlayer, leftPlayer, config.aIConfig);
        }
        else
        {
            var input = new InputController(1);
            return new HumanController(input);
        }
    }

    private void Referee_OnInteraction(object sender, RefereeEventArgs e)
    {
        if (e.type == RefereeEventType.Win)
        {
            GameInProgress.Instance.WinnerId = e.reciever == EventSource.LEFT ? 0 : 1;
            IsStarted = false;
            SceneManager.LoadScene("GameOver");
        }
    }
}