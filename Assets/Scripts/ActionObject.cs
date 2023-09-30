using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(
    fileName = nameof(ActionObject),
    menuName =  "ScriptableObjects/" + nameof(ActionObject))
]
public class ActionObject : ScriptableObject
{
    [field: SerializeField]
    public List<State_duration> States { get; private set; }

    [field: SerializeField]
    public int StaminaCost { get; private set; }
   
}
