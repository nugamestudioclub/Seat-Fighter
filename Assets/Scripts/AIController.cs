using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : IActionProvider
{

    public Queue<Action_state> playerQueue;
    public Queue<Action_state> aiQueue;

    private Action_state[] oldQueue;

    [SerializeField]
    private List<ActionResponse> actionResponses;

    [SerializeField]
    private ActionObject push;

    AiAction curAction = AiAction.EMPTY;

    public Action GetNextAction()
    {
        Action toReturn = Action.None;
        Action_state[] curQueue = playerQueue.ToArray();
        if (!(curAction.Equals(AiAction.EMPTY)))
        {
            if (curAction.checkDelay())
            {
                toReturn = curAction.Action;
            }
        }
        else if (aiQueue.Count == 0)
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

    private bool isNewActionTaken(Action_state[] curQueue) {
        return oldQueue.Length == 0 && curQueue.Length > 0;
    }

    private OpponentAction findOpponentAction(Action_state[] curQueue)
    {
        if (curQueue[0] == Action_state.COOLDOWN)
        {
            return new OpponentAction(Action_state.COOLDOWN, curQueue.Length);
        }
        for (int i = 0; i < curQueue.Length; i++)
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
