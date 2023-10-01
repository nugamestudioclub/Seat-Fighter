using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public void Bind(Referee referee, Player leftPlayer, Player rightPlayer, Environment environment)
    {
        referee.RefereeEvent += Referee_OnInteraction;
        leftPlayer.PlayerEvent += Player_OnChange;
        rightPlayer.PlayerEvent += Player_OnChange;
        environment.EnvironmentChangeEvent += Environment_OnChange;
    }

    private void Player_OnChange(object sender, PlayerEventArgs e)
    {
        
    }

    private void Environment_OnChange(object sender, EnvironmentEventArgs e)
    {
        
    }
    private void Referee_OnInteraction(object sender, RefereeEventArgs e)
    {
        switch(e.type)
        {
            case RefereeEventType.Dodge:

                break;
            case RefereeEventType.ShovePush:
            case RefereeEventType.ShoveContact:

                break;
            case RefereeEventType.StartBlock:

                break;
            case RefereeEventType.StartShove:

                break;
            case RefereeEventType.StartPush:

                break;
            case RefereeEventType.ShoveBlock:

                break;
        }
    }
}
