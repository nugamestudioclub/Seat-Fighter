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
		int winnerId = GameInProgress.Instance.WinnerId;
		var config =  GameInProgress.Instance.Config;
		string winnerName = (winnerId == 0 ? config.leftPlayerConfig : config.rightPlayerConfig).characterName;

		Debug.Log("winner " + winnerName);

		if( winnerId == 0 ) {
			leftName.text = winnerName;
			leftMessage.SetActive(true);
			rightMessage.SetActive(false);
		}
		else {
			rightName.text = winnerName;
			leftMessage.SetActive(false);
			rightMessage.SetActive(true);
		}
	}

	public void MainMenu() {
		SceneManager.LoadScene(0);
	}
	public void PlayAgain() {
		SceneManager.LoadScene(1);
	}

	public void Exit() {
		Application.Quit();
	}
}
