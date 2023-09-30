using UnityEngine;

public class PlayerController : IActionProvider
{
    public Player player { get; private set; }

    private PlayerInput input;

    public PlayerController(Player player, PlayerInput input)
    {
        this.player = player;
        this.input = input;
    }

    public Action GetNextAction()
    {
        Action a = input.GetNextAction();
        if (player.Current_action == Action_state.IDLE)
        {
            // TODO poll user input
        }
        return a;
    }
}
