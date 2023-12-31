using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(
    fileName = nameof(AIConfig),
    menuName = "ScriptableObjects/" + nameof(AIConfig))
]
public class AIConfig : ScriptableObject
{
    public List<ActionResponse> actionResponses;
    public float waitRollThreshold = .35f;
    public float waitRollChance = .70f;
}
