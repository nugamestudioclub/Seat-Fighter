using System;
using System.Collections;
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
    
    private bool isAI = false;
    private void Awake()
    {
        inputController_left = new InputController(0);
        inputController_right = new InputController(1);
    }

    private void Start()
    {
        gameInProgress = GameInProgress.Instance;
        players = gameInProgress.AllPlayers.Options;
        if (gameInProgress.PlayerCount == 1)
        {
            rightPlayerIndex = Mathf.FloorToInt(UnityEngine.Random.Range(0, players.Count));
            UpdateSprites();
            isAI = true;
        }
        
        
    }

    private void Update()
    {
        if (inputController_right.InputData.GetButtonState(Button.Start).IsDown || inputController_left.InputData.GetButtonState(Button.Start).IsDown)
        {
            SceneManager.LoadScene("MainScene");
        }
        
        if (inputController_left.InputData.Direction.x > 0)
        {
            Debug.Log("Left input");
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
        else if (inputController_left.InputData.Direction.x < 0)
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
            if (inputController_right.InputData.Direction.x > 0)
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
            else if (inputController_right.InputData.Direction.x < 0)
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

    private void UpdateSprites()
    {
        leftPlayerSprite.sprite = players[leftPlayerIndex].portrait;
        rightPlayerSprite.sprite = players[rightPlayerIndex].portrait;
        leftPlayerSprite.gameObject.GetComponentsInChildren<TextMeshPro>()[0].text = players[leftPlayerIndex].name;
        rightPlayerSprite.gameObject.GetComponentsInChildren<TextMeshPro>()[0].text = players[rightPlayerIndex].name;
        
        // Update GameInProgress
        gameInProgress.LeftPlayer = players[leftPlayerIndex];
        gameInProgress.RightPlayer = players[rightPlayerIndex];
    }
}