using UnityEngine;

public class MainMenu : MonoBehaviour {
	public void Set1Player() {
		GameInProgress.Instance.PlayerCount = 1;
	}

	public void Set2Player() {
		GameInProgress.Instance.PlayerCount = 2;
	}
}