using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {
	[SerializeField] private EventSystem eventsystem;
	[SerializeField] private GameObject cursor;

	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private TMPro.TMP_Text leftName;

	[SerializeField]
	private TMPro.TMP_Text rightName;

	[SerializeField]
	private GameObject leftMessage;

	[SerializeField]
	private GameObject rightMessage;

	Vector3 messagePosition;

	private void Awake() {
	}

	private void Start() {
		SetWinner();
	}

	public void Update() {
		cursor.transform.position = new Vector2(
			cursor.transform.position.x,
			eventsystem.currentSelectedGameObject.transform.position.y
		);
	}

	private void SetWinner() {
		Debug.Log(nameof(SetWinner));
		var gameInProgress = GameInProgress.Instance;
		var winnerId = gameInProgress.WinnerId;
		var winner = (winnerId == 0 ? gameInProgress.LeftPlayer : gameInProgress.RightPlayer);

		Debug.Log(winnerId);
		Debug.Log(winner);

		if( winnerId == 0 ) {
			leftName.text = winner.characterName;
			leftMessage.SetActive(true);
			rightMessage.SetActive(false);
		}
		else {
			rightName.text = winner.characterName;
			leftMessage.SetActive(false);
			rightMessage.SetActive(true);
		}
	}

	public void MainMenu() {
		SceneManager.LoadScene("TitleScreen");
	}
	public void PlayAgain() {
		SceneManager.LoadScene("MainScene");
	}

	public void Exit() {
		Application.Quit();
	}
}
