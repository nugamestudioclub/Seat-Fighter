using UnityEngine;

public class Bubble : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer _fill;

	[SerializeField]
	private SpriteRenderer _stroke;

	[SerializeField]
	private TMPro.TMP_Text _text;

	public void SetEnabled(bool value) {
		enabled = value;
	}

	public void SetFillColor(Color color) {
		_fill.color = color;
	}

	public void SetFillSprite(Sprite sprite) {
		_fill.sprite = sprite;
	}

	public void SetPosition(Vector3 position) {
		transform.position = position;
	}

	public void SetRotation(Vector3 rotation) {
		transform.Rotate(rotation);
	}

	public void SetScale(Vector3 scale) {
		transform.localScale = scale;
	}

	public void SetShapeScale(Vector3 scale) {
		_fill.transform.localScale = scale;
	}
	
	public void SetStrokeColor(Color color) {
		_stroke.color = color;
	}

	public void SetStrokeSprite(Sprite sprite) {
		_stroke.sprite = sprite;
	}

	public void SetText(string text) {
		_text.text = text;
	}

	public void SetTextColor(Color color) {
		_text.color= color;
	}
}