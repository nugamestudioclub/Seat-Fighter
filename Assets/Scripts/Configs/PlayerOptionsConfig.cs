using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
	fileName = nameof(PlayerOptionsConfig),
	menuName = "ScriptableObjects/" + nameof(PlayerOptionsConfig))
]
public class PlayerOptionsConfig : ScriptableObject {
	[SerializeField]
	private List<PlayerConfig> options;

	public IList<PlayerConfig> Options => options;
}