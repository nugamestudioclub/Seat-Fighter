using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
	fileName = nameof(SpecialEffectConfig),
	menuName = "ScriptableObjects/" + nameof(SpecialEffectConfig))
]
public class SpecialEffectConfig : ScriptableObject {
	[field: SerializeField]
	public List<ActionSpecialEffect> Actions { get; private set; }

	[field: SerializeField]
	public List<InteractionSpecialEffect> Interactions { get; private set; }
}