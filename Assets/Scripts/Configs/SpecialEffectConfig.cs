using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
	fileName = nameof(SpecialEffectConfig),
	menuName = "ScriptableObjects/" + nameof(SpecialEffectConfig))
]
public class SpecialEffectConfig : ScriptableObject
{
	[SerializeField] public List<ActionSpecialEffect> actions;

	[SerializeField] public List<InteractionSpecialEffect> interactions;

    [SerializeField] public List<PlayerStateSpecialEffect> states;
}