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
        referee.Interaction += Referee_OnInteraction;
    }

    private void Referee_OnInteraction(object sender, InteractionEventArgs e)
    {
        if (e.type == EventType.OutOfBounds)
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
        else if (e.type == EventType.PushContact)
        {
            if (e.reciever == EventSource.LEFT)
            {
                environment.Position = Math.Max(
                    -config.environmentConfig.edgeDistance,
                    environment.Position - config.rightPlayerConfig.pushDamage);
            }
            else
            {
                environment.Position = Math.Min(
                    config.environmentConfig.edgeDistance + config.environmentConfig.armrestWidth,
                    environment.Position + config.leftPlayerConfig.pushDamage);
            }
        }
        else if (e.type == EventType.ShoveContact)
        {
            if (e.reciever == EventSource.LEFT)
            {
                environment.Position = Math.Max(
                    -config.environmentConfig.edgeDistance,
                    environment.Position - config.rightPlayerConfig.shoveDamage);
            }
            else
            {
                environment.Position = Math.Min(
                    config.environmentConfig.edgeDistance + config.environmentConfig.armrestWidth,
                    environment.Position + config.leftPlayerConfig.shoveDamage);
            }
        }
    }
}
