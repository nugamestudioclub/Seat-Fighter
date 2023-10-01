using System;
using System.Collections.Generic;
using UnityEngine;

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

    public static GameManager Instance => _instance;

    private long fixedFrameCount;

    [SerializeField]
    private int frameInterval = 5;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        leftPlayer.Update();
        rightPlayer.Update();
    }

    private void FixedUpdate()
    {
        fixedFrameCount++;
        if (fixedFrameCount % frameInterval == 0)
        {
            referee.Tick();
        }
    }

    private void Initialize()
    {
        fixedFrameCount = 0;
        //create player and environment from the configs
        leftPlayer = new Player(config.leftPlayerConfig, EventSource.LEFT);
        rightPlayer = new Player(config.rightPlayerConfig, EventSource.RIGHT);

        _inputController.Initialize();
        humanController = new(_inputController);
        var aiController = new AIController(rightPlayer, leftPlayer, config.aIConfig);

        leftPlayer.Bind(humanController);
        rightPlayer.Bind(aiController);

        Environment environment = new Environment(
            config.environmentConfig,
            config.leftPlayerConfig.health,
            config.rightPlayerConfig.health);

        referee = new Referee(leftPlayer, rightPlayer, environment);
        logic = new GameLogic(config, leftPlayer, rightPlayer, environment);
        logic.Bind(referee);

        view.Bind(environment, leftPlayer, rightPlayer);
        audioManager.Bind(referee, leftPlayer, rightPlayer, environment, config.specialEffectConfig);
    }
}