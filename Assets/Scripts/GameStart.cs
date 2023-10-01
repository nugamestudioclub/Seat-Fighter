using UnityEngine;

public class GameStart : MonoBehaviour {
	void Start() {
		GameManager.Instance.Play();
	}
}