using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class CharSel : MonoBehaviour
{
    private InputController inputController_left;
    private InputController inputController_right;

    private GameInProgress gameInProgress;

    private int leftPlayerIndex = 0;
    private int LeftPlayerIndex
    {
        get => leftPlayerIndex;
        set
        {
            leftPlayerIndex = value;
            UpdateSprites();
        }
    }
    private int rightPlayerIndex = 0;
    private int RightPlayerIndex
    {
        get => rightPlayerIndex;
        set
        {
            rightPlayerIndex = value;
            UpdateSprites();
        }
    }
    private IList<PlayerConfig> characters;

    [SerializeField] private SpriteRenderer leftPlayerSprite;
    [SerializeField] private SpriteRenderer rightPlayerSprite;

    [SerializeField] private TMP_Text leftPlayerNameLabel;
    [SerializeField] private TMP_Text rightPlayerNameLabel;

    [SerializeField] private TMP_Text leftCharacterNameLabel;
    [SerializeField] private TMP_Text rightCharacterNameLabel;

    [SerializeField] private TMP_Text pressStartLabel;

    [SerializeField] private GameObject[] rightPlayerArrows;
    [SerializeField] private GameObject[] leftPlayerArrows;

    [SerializeField]
    private int frameInterval = 5;
    private int fixedFrameCount;
    [SerializeField]
    private float timeTillStartIsEnabled = 1.0f;

    private bool isLeftPlayerReady;
    private bool IsLeftPlayerReady
    {
        get => isLeftPlayerReady;
        set
        {
            if (!value)
            {
                ResetMenu();
            }
            SetPlayerReadyUIActive(0, !value);
            isLeftPlayerReady = value;
        }
    }
    private bool isRightPlayerReady;
    private bool IsRightPlayerReady
    {
        get => isRightPlayerReady;
        set
        {
            if (!value)
            {
                ResetMenu();
            }
            SetPlayerReadyUIActive(1, !value);
            isRightPlayerReady = value;
        }
    }

    private void SetPlayerReadyUIActive(int player, bool active)
    {
        foreach (GameObject arrow in player == 0 ? leftPlayerArrows : rightPlayerArrows)
        {
            arrow.SetActive(active);
        }
    }

    private float lastLeftStart;
    private float lastRightStart;


    private Vector2 leftDesiredDirection;
    private Vector2 rightDesiredDirection;
    private bool isAI;
    private void Awake()
    {
        fixedFrameCount = 0;
        inputController_left = new InputController(0);
        inputController_right = new InputController(1);
        IsLeftPlayerReady = false;
        IsRightPlayerReady = false;
        isAI = false;
        ResetMenu();
        lastLeftStart = 0;
        lastRightStart = 0;
    }

    private void ResetMenu()
    {
        pressStartLabel.text = "Press START to select"; ;
    }

    private void Start()
    {
        gameInProgress = GameInProgress.Instance;
        characters = gameInProgress.AllPlayers.Options;

        RightPlayerIndex = GetOtherPlayerIndex(0, characters.Count);
        if (gameInProgress.PlayerCount == 1)
        {
            leftPlayerNameLabel.text = "Player";
            rightPlayerNameLabel.text = "Computer";
            isAI = true;
            SetPlayerReadyUIActive(1, false);
        }
        else
        {
            leftPlayerNameLabel.text = "Player 1";
            rightPlayerNameLabel.text = "Player 2";
        }
        UpdateSprites();
    }

    private int GetOtherPlayerIndex(int playerIndex, int numPlayers)
    {
        System.Random random = new();
        int candidate = playerIndex;
        while (candidate == playerIndex)
        {
            candidate = random.Next(0, numPlayers);
        }
        return candidate;
    }

    private bool HandleLeftStartInput()
    {
        bool ableTomove = lastLeftStart + timeTillStartIsEnabled < Time.timeSinceLevelLoad;
        bool willMove = inputController_left.InputData.GetButtonState(Button.Start).IsDown && ableTomove;
        if (willMove)
        {
            lastLeftStart = Time.timeSinceLevelLoad;
        }
        return willMove;
    }

    private bool HandleRightStartInput()
    {
        if (isAI) { return false; }
        bool ableTomove = lastRightStart + timeTillStartIsEnabled < Time.timeSinceLevelLoad;
        bool willMove = inputController_right.InputData.GetButtonState(Button.Start).IsDown && ableTomove;
        if (willMove)
        {
            lastRightStart = Time.timeSinceLevelLoad;
        }
        return willMove;
    }

    private void Update()
    {
        leftDesiredDirection = inputController_left.InputData.Direction;
        rightDesiredDirection = inputController_right.InputData.Direction;
        if (inputController_left.InputData.GetButtonState(Button.Cancel).IsDown)
        {
            if (isAI && isRightPlayerReady)
            {
                IsRightPlayerReady = false;
            }
            else
            {
                if (isAI)
                {
                    SetPlayerReadyUIActive(1, false);
                }
                IsLeftPlayerReady = false;
            }
        }
        if (!isAI && inputController_right.InputData.GetButtonState(Button.Cancel).IsDown)
        {
            IsRightPlayerReady = false;
        }
        if (Time.timeSinceLevelLoad > timeTillStartIsEnabled)
        {
            if (IsLeftPlayerReady && IsRightPlayerReady)
            {
                pressStartLabel.text = "Press START to begin!";
                if (HandleLeftStartInput() || HandleRightStartInput())
                {
                    GameInProgress.Instance.LoadScene("MainScene");
                }
            }
            if (HandleLeftStartInput())
            {
                if (IsLeftPlayerReady)
                {
                    if (isAI)
                    {
                        IsRightPlayerReady = true;
                    }
                }
                else
                {
                    IsLeftPlayerReady = true;
                    if (isAI)
                    {
                        IsRightPlayerReady = false;
                    }
                }
            }
            if (HandleRightStartInput())
            {
                IsRightPlayerReady = true;
            }
            

        }
    }

    private bool LeftPlayerMoveLeft => leftDesiredDirection.x > 0 || leftDesiredDirection.y > 0;
    private bool LeftPlayerMoveRight => leftDesiredDirection.x < 0 || leftDesiredDirection.y < 0;
    private bool RightPlayerMoveLeft => rightDesiredDirection.x > 0 || rightDesiredDirection.y > 0;
    private bool RightPlayerMoveRight => leftDesiredDirection.x < 0 || leftDesiredDirection.y < 0;

    private void FixedUpdate()
    {
        fixedFrameCount++;
        if (fixedFrameCount % frameInterval == 0)
        {
            if (!IsLeftPlayerReady)
            {
                if (LeftPlayerMoveLeft)
                {
                    LeftPlayerIndex = (LeftPlayerIndex - 1 + characters.Count) % characters.Count;
                }
                else if (LeftPlayerMoveRight)
                {
                    LeftPlayerIndex = (LeftPlayerIndex + 1) % characters.Count;
                }
            }
            else if (isAI && !IsRightPlayerReady)
            {
                if (LeftPlayerMoveLeft)
                {
                    RightPlayerIndex = (RightPlayerIndex + characters.Count - 1) % characters.Count;
                }
                else if (LeftPlayerMoveRight)
                {
                    RightPlayerIndex = (RightPlayerIndex + 1) % characters.Count;
                }
            }


            if (!isAI && !IsRightPlayerReady)
            {
                if (RightPlayerMoveLeft)
                {
                    RightPlayerIndex = (RightPlayerIndex + characters.Count - 1) % characters.Count;
                }
                else if (RightPlayerMoveRight)
                {
                    RightPlayerIndex = (RightPlayerIndex + 1) % characters.Count;
                }
            }
        }
    }
    private void UpdateSprites()
    {
        leftPlayerSprite.sprite = characters[LeftPlayerIndex].portrait;
        rightPlayerSprite.sprite = characters[RightPlayerIndex].portrait;
        leftCharacterNameLabel.text = characters[LeftPlayerIndex].characterName;
        rightCharacterNameLabel.text = characters[RightPlayerIndex].characterName;

        // Update GameInProgress
        gameInProgress.LeftPlayer = characters[LeftPlayerIndex];
        gameInProgress.RightPlayer = characters[RightPlayerIndex];
    }
}