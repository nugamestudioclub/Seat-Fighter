using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{

    public Queue<Action_state> playerQueue;
    public Queue<Action_state> aiQueue;

    private Action_state[] oldQueue;

    [SerializeField]
    private List<ActionResponse> actionResponses;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Action_state[] curQueue = playerQueue.ToArray();
        if (aiQueue.Count == 0)
        {
            if (isNewActionTaken(curQueue))
            {
                OpponentAction action = findOpponentAction(curQueue);
                AiAction aiAction = GetReponseAction(action);
            }
        }


        oldQueue = curQueue;
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

    private void DoAction(AiAction action)
    {
        DoAction(action, 0);
    }

    private void DoAction(AiAction action, int delay)
    {
        // TODO
    }

}
