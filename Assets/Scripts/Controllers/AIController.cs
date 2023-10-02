using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityRandom = UnityEngine.Random;

public class AIController : IActionProvider
{
    private List<ActionState> oldQueue;

    private Player player;
    private Player enemy;

    private AIConfig config;
   [SerializeField]
    private List<ActionResponse> actionResponses;

    public List<ActionResponse> ActionResponses => config.actionResponses;
    private float waitRollThreshold => config.waitRollThreshold;
    private float waitRollChance => config.waitRollChance;

    AIAction curAction = AIAction.EMPTY;

    public AIController(Player player, Player enemy, AIConfig config)
    {
        this.player = player;
        this.enemy = enemy;
        this.config = config;
        oldQueue = new();
    }

    public Action GetNextAction()
    {
        Action toReturn = Action.None;

        List<ActionState> curQueue = enemy.ActionList.Select(action => action.state).ToList();

        if (curAction.Equals(AIAction.EMPTY)) {
            if (player.CurrentFrameData.state == ActionState.IDLE) { 
                OpponentAction action = findOpponentAction(curQueue);
                curAction = GetReponseAction(action);
            }
        }
        else
        {
            if (curAction.CheckDelay())
            {
                toReturn = curAction.Action;
                if (!curAction.HoldAction())
                {
                    curAction = AIAction.EMPTY;
                }
            }
        }

        oldQueue = curQueue;
        return toReturn;
    }

    private bool isNewActionTaken(List<ActionState> curQueue) {
        return oldQueue.Count == 0 && curQueue.Count > 0;
    }

    private OpponentAction findOpponentAction(List<ActionState> curQueue)
    {
        if (curQueue.Count == 0)
        {
            return new OpponentAction(ActionState.IDLE, 0);
        }
        if (curQueue[0] == ActionState.COOLDOWN)
        {
            return new OpponentAction(ActionState.COOLDOWN, curQueue.Count);
        }
        for (int i = 0; i < curQueue.Count; i++)
        {
            if (curQueue[i] != ActionState.BUSY)
            {
                return new OpponentAction(curQueue[i], i);
            }
        }
        return new OpponentAction(ActionState.IDLE, 0);
    }

    private AIAction GetReponseAction(OpponentAction opponentAction)
    {
        for (int i = 0; i < ActionResponses.Count; i++)
        {
            if (ActionResponses[i].ActionState == opponentAction.Action)
            {
                if ((float) player.Stamina / (float) player.maxStamina <= waitRollThreshold 
                    && UnityRandom.Range(0f, 1f) <= waitRollChance)
                {
                    return ActionResponses[i].GetWaitResponse();
                }
                return ActionResponses[i].GetResponse();
            }
        }
        throw new Exception("You forgot a response action didn't you");
    }

}
