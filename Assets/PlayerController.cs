using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Player playerData;

    [SerializeField]
    private ActionObject push;

    private void Awake()
    {
        playerData = new Player(push, push, push, push);
    }

    public void FixedUpdate()
    {
        Action a = Action.None;
        if (playerData.Current_action == Action_state.IDLE)
        {
            // TODO add InputController stuff
            switch(a) {
                case Action.Shove: playerData.Shove(); break;
                case Action.Dodge: playerData.Dodge(); break;
                case Action.Push: playerData.Push(); break;
                case Action.Block: playerData.Block(); break;
                case Action.None: break;
            }
        }
        playerData.Tick();
    }
}
