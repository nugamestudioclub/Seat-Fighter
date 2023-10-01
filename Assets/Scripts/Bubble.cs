using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Bubble : MonoBehaviour {
	[SerializeField]
	private SpriteRenderer _fill;

	[SerializeField]
	private SpriteRenderer _stroke;

	[SerializeField]
	private TMPro.TMP_Text _text;


	public void SetColor(Color color) {
		_fill.color = color;
		_stroke.color = color;
		_text.color= color;
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

	public void SetText(string text) {
		_text.text = text;
	}
}