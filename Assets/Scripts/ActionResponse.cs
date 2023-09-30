using UnityRandom = UnityEngine.Random;
using System.Collections.Generic;
using System;

[Serializable]
public struct ActionResponse
{
    public Action_state ActionState;
    public List<WeightedItem> weightedList;

    public AiAction GetResponse()
    {
        List<AiAction> actionPool = new List<AiAction>();
        for (int i = 0; i < weightedList.Count; i++)
        {
            for (int j = 0; j < weightedList[i].Weight; j++)
            {
                actionPool.Add(weightedList[i].AiAction);
            }
        }
        int rand = UnityRandom.Range(0, actionPool.Count);
        return actionPool[rand];
    }

}