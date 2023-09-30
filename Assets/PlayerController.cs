using UnityEngine;

public class PlayerController : IActionProvider
{
    [SerializeField]
    private ActionObject push;

    private Player player;

    PlayerController(Player player)
    {
        this.player = player;
    }

    public Action GetNextAction()
    {
        Action a = Action.None;
        if (player.Current_action == Action_state.IDLE)
        {
            // TODO poll user input
        }
        return a;
    }
}
