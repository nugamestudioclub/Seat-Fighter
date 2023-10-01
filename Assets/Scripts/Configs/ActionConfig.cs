using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(ActionConfig),
    menuName =  "ScriptableObjects/" + nameof(ActionConfig))
]
public class ActionConfig : ScriptableObject
{
    [field: SerializeField]
    public List<StateDuration> States { get; private set; }

    [field: SerializeField]
    public List<SpriteDuration> Sprites { get; private set; }

    [field: SerializeField]
    public int StaminaCost { get; private set; }
   
    public ActionConfig(List<StateDuration> states, int staminaCost)
    {
        States = states;
        StaminaCost = staminaCost;
    }
}
