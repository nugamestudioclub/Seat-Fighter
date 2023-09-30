using UnityEngine;
using System;

public class GameLogic : MonoBehaviour
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
                environment.leftPlayerTime--;
            }
            else
            {
                environment.rightPlayerTime--;
            }
        }
        else if (e.type == EventType.PushContact)
        {
            if (e.reciever == EventSource.LEFT)
            {
                environment.position = Math.Max(
                    -config.environmentConfig.edgeDistance,
                    environment.position - config.rightPlayerConfig.pushDamage);
            }
            else
            {
                environment.position = Math.Min(
                    config.environmentConfig.edgeDistance + config.environmentConfig.armrestWidth,
                    environment.position + config.leftPlayerConfig.pushDamage);
            }
        }
        else if (e.type == EventType.ShoveContact)
        {
            if (e.reciever == EventSource.LEFT)
            {
                environment.position = Math.Max(
                    -config.environmentConfig.edgeDistance,
                    environment.position - config.rightPlayerConfig.shoveDamage);
            }
            else
            {
                environment.position = Math.Min(
                    config.environmentConfig.edgeDistance + config.environmentConfig.armrestWidth,
                    environment.position + config.leftPlayerConfig.shoveDamage);
            }
        }
    }
}
