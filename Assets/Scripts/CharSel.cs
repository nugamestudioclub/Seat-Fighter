using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Vector2 leftDesiredDirection;
    private Vector2 rightDesiredDirection;
    private List<bool> leftDesiredInputData = new();
    private List<bool> rightDesiredInputData = new();
    private bool isAI = false;
    private void Awake()
    {
        for (int i = 0; i < Enum.GetValues(typeof(Button)).Length; i++)
        {
            leftDesiredInputData.Add(new ButtonState());
            rightDesiredInputData.Add(new ButtonState());
        }
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

        for (int i = 0; i < leftDesiredInputData.Count; i++)
        {

            if (inputController_left.InputData.ButtonStates[i].IsDown)
            {
                leftDesiredInputData[i] = true;
            }
            if (inputController_right.InputData.ButtonStates[i].IsDown)
            {
                rightDesiredInputData[i] = true;
            }
        }
    }
    private void FixedUpdate()
    {
        if (Time.timeSinceLevelLoad > 1)
        {


            fixedFrameCount++;
            if (fixedFrameCount % frameInterval == 0)
            {
                if (leftDesiredInputData[(int)Button.Start]
        || rightDesiredInputData[(int)Button.Start])
                {

                    SceneManager.LoadScene("MainScene");
                }
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
        leftPlayerSprite.sprite = players[leftPlayerIndex].portrait;
        rightPlayerSprite.sprite = players[rightPlayerIndex].portrait;
        leftPlayerNameLabel.text = players[leftPlayerIndex].characterName;
        rightPlayerNameLabel.text = players[rightPlayerIndex].characterName;

        // Update GameInProgress
        gameInProgress.LeftPlayer = players[leftPlayerIndex];
        gameInProgress.RightPlayer = players[rightPlayerIndex];
    }
}