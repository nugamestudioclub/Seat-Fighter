using UnityEngine;
using System;

public class GameLogic
{
    private GameConfig config;

    private Player leftPlayer;
    private Player rightPlayer;
    private Environment environment;

    public GameLogic(GameConfig config, Player leftPlayer, Player rightPlayer, Environment environment)
    {
        this.config = config;
        this.leftPlayer = leftPlayer;
        this.rightPlayer = rightPlayer;
        this.environment = environment;
    }

    public void Bind(Referee referee)
    {
        referee.RefereeEvent += Referee_OnInteraction;
    }

    private void Referee_OnInteraction(object sender, RefereeEventArgs e)
    {
        if(e.type == RefereeEventType.Win)
        {
            if (e.reciever == EventSource.LEFT)
            {
                //DISPLAY PLAYER 1 (JORDAN) WIN
                Debug.Log("PLAYER 1 WIN");
            }
            else
            {
                //DISPLAY PLAYER 2 (GORDON) WIN
                Debug.Log("PLAYER 2 WIN");
            }
        }
        if (e.type == RefereeEventType.OutOfBounds)
        {
            if (e.reciever == EventSource.LEFT)
            {
                environment.LeftPlayerTime--;
            }
            else
            {
                environment.RightPlayerTime--;
            }
        }
        else if (e.type == RefereeEventType.PushIdle)
        {
            if (e.reciever == EventSource.LEFT)
            {
               environment.Position -= config.rightPlayerConfig.push.positionModifier;
            }
            else
            {
               environment.Position += config.leftPlayerConfig.push.positionModifier;
            }
        }
        else if (e.type == RefereeEventType.ShoveIdle)
        {
            if (e.reciever == EventSource.LEFT)
            {
                environment.Position -= config.rightPlayerConfig.shove.positionModifier;
            }
            else
            {
                environment.Position += config.leftPlayerConfig.shove.positionModifier;
            }
        }
    }
}
