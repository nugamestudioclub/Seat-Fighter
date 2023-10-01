using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
	fileName = nameof(BubbleOptionsConfig),
	menuName = "ScriptableObjects/" + nameof(BubbleOptionsConfig))
]
public class BubbleOptionsConfig : ScriptableObject {
	[field: SerializeField]
	public List<BubbleOption> Options { get; private set; }

	internal object Select(Func<object, (object, object)> value) {
		throw new NotImplementedException();
	}
}