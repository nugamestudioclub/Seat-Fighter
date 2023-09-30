using System;
using System.Collections.Generic;
using UnityEngine;

public class AIController : IActionProvider
{
    private List<Action_state> oldQueue;

    private Player player;
    private Player enemy;

    [SerializeField]
    private List<ActionResponse> actionResponses;

    AiAction curAction = AiAction.EMPTY;

    public AIController(Player player, Player enemy)
    {
        this.player = player;
        this.enemy = enemy;
    }

    public Action GetNextAction()
    {
        Action toReturn = Action.None;
        List <Action_state> curQueue = enemy.ActionList;
        if (!(curAction.Equals(AiAction.EMPTY)))
        {
            if (curAction.checkDelay())
            {
                toReturn = curAction.Action;
            }
        }
        else if (player.Current_action == Action_state.IDLE)
        {
            if (isNewActionTaken(curQueue))
            {
                OpponentAction action = findOpponentAction(curQueue);
                curAction = GetReponseAction(action);
            }
        }

        oldQueue = curQueue;
        return toReturn;
    }

    private bool isNewActionTaken(List<Action_state> curQueue) {
        return oldQueue.Count == 0 && curQueue.Count > 0;
    }

    private OpponentAction findOpponentAction(List<Action_state> curQueue)
    {
        if (curQueue[0] == Action_state.COOLDOWN)
        {
            return new OpponentAction(Action_state.COOLDOWN, curQueue.Count);
        }
        for (int i = 0; i < curQueue.Count; i++)
        {
            if (curQueue[i] != Action_state.BUSY)
            {
                return new OpponentAction(curQueue[i], i);
            }
        }
        throw new System.Exception("Something messed up when finding actions");
    }

    private AiAction GetReponseAction(OpponentAction opponentAction)
    {
        for (int i = 0; i < actionResponses.Count; i++)
        {
            if (actionResponses[i].ActionState == opponentAction.Action)
            {
                return actionResponses[i].GetResponse();
            }
        }
        throw new Exception("You forgot a response action didn't you");
    }

}
