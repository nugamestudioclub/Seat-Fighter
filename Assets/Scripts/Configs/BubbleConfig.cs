using UnityEngine;

[CreateAssetMenu(
	fileName = nameof(BubbleConfig),
	menuName = "ScriptableObjects/" + nameof(BubbleConfig))
]
public class BubbleConfig : ScriptableObject {
	[SerializeField]
	public Sprite FillSprite { get; private set; }

	[field: SerializeField]
	public Color FillColor { get; private set; }

	[SerializeField]
	public Sprite StrokeSprite { get; private set; }

	[field: SerializeField]
	public Color StrokeColor { get; private set; }

	[field: SerializeField]
	public string Text { get; set; }

	[field: SerializeField]
	public Color TextColor { get; private set; }

	[field: SerializeField]
	public Vector3 ShapeScale { get; private set; } = Vector3.one;
}