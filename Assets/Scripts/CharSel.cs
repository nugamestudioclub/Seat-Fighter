using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSel : MonoBehaviour
{
    private InputController inputController_left;
    private InputController inputController_right;

    private GameInProgress gameInProgress;

    private int leftPlayerIndex = 0;
    private int rightPlayerIndex = 0;
    private IList<PlayerConfig> players;

    [SerializeField] private SpriteRenderer leftPlayerSprite;
    [SerializeField] private SpriteRenderer rightPlayerSprite;

    [SerializeField] private TMP_Text leftPlayerNameLabel;
    [SerializeField] private TMP_Text rightPlayerNameLabel;

    [SerializeField]
    private int frameInterval = 5;
    private int fixedFrameCount;
    [SerializeField]
    private float timeTillInputIsEnabled = 1.0f;

    private Vector2 leftDesiredDirection;
    private Vector2 rightDesiredDirection;
    private bool isAI = false;
    private void Awake()
    {
        fixedFrameCount = 0;
        inputController_left = new InputController(0);
        inputController_right = new InputController(1);
    }

    private void Start()
    {
        gameInProgress = GameInProgress.Instance;
        players = gameInProgress.AllPlayers.Options;
        if (gameInProgress.PlayerCount == 1)
        {
            rightPlayerIndex = GetOtherPlayerIndex(0, players.Count);
            isAI = true;
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

    private void Update()
    {
        leftDesiredDirection = inputController_left.InputData.Direction;
        rightDesiredDirection = inputController_right.InputData.Direction;

        if (Time.timeSinceLevelLoad > timeTillInputIsEnabled)
        {
            if (inputController_right.InputData.GetButtonState(Button.Start).IsDown
            || inputController_left.InputData.GetButtonState(Button.Start).IsDown)
            {

                SceneManager.LoadScene("MainScene");
            }
        }
    }
    private void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad > timeTillInputIsEnabled)
        {
            fixedFrameCount++;
            if (fixedFrameCount % frameInterval == 0)
            {

                if (leftDesiredDirection.x > 0 || leftDesiredDirection.y > 0)
                {
                    if (leftPlayerIndex < players.Count - 1)
                    {
                        leftPlayerIndex++;
                    }
                    else
                    {
                        leftPlayerIndex = 0;
                    }

                    UpdateSprites();
                }
                else if (leftDesiredDirection.x < 0 || leftDesiredDirection.y < 0)
                {
                    if (leftPlayerIndex > 0)
                    {
                        leftPlayerIndex--;
                    }
                    else
                    {
                        leftPlayerIndex = players.Count - 1;
                    }

                    UpdateSprites();
                }

                if (isAI == false)
                {
                    if (rightDesiredDirection.x > 0 || rightDesiredDirection.y > 0)
                    {
                        if (rightPlayerIndex < players.Count - 1)
                        {
                            rightPlayerIndex++;
                        }
                        else
                        {
                            rightPlayerIndex = 0;
                        }

                        UpdateSprites();
                    }
                    else if (rightDesiredDirection.x < 0 || rightDesiredDirection.y < 0)
                    {
                        if (rightPlayerIndex > 0)
                        {
                            rightPlayerIndex--;
                        }
                        else
                        {
                            rightPlayerIndex = players.Count - 1;
                        }

                        UpdateSprites();
                    }
                }
            }
        }
    }
    private void UpdateSprites()
    {
        if (isAI && leftPlayerIndex == rightPlayerIndex)
        {
            rightPlayerIndex = GetOtherPlayerIndex(leftPlayerIndex, players.Count);
        }
        leftPlayerSprite.sprite = players[leftPlayerIndex].portrait;
        rightPlayerSprite.sprite = players[rightPlayerIndex].portrait;
        leftPlayerNameLabel.text = players[leftPlayerIndex].characterName;
        rightPlayerNameLabel.text = players[rightPlayerIndex].characterName;

        // Update GameInProgress
        gameInProgress.LeftPlayer = players[leftPlayerIndex];
        gameInProgress.RightPlayer = players[rightPlayerIndex];


    }
}