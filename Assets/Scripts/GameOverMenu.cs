using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {
	[SerializeField]
	private EventSystem _eventSystem;

	[SerializeField]
	private GameObject cursor;

	private GameObject _currentSelection;

	[SerializeField]
	private SpriteRenderer leftPlayerPortrait;
    [SerializeField]
    private SpriteRenderer rightPlayerPortrait;

    private GameObject CurrentSelection {
		get => _currentSelection;
		set {
			if( value == null )
				_eventSystem.SetSelectedGameObject(_currentSelection);
			else
				_currentSelection = value;
		}
	}

	[SerializeField]
	private TMPro.TMP_Text leftName;

	[SerializeField]
	private TMPro.TMP_Text rightName;

	[SerializeField]
	private GameObject leftMessage;

	[SerializeField]
	private GameObject rightMessage;

	Vector3 messagePosition;

	[SerializeField]
	private AudioHandler audioHandler;

	private void Start() {
		SetWinner();
	}

	void Update() {
		UpdateSelection();
	}

	private void SetWinner() {
		Debug.Log(nameof(SetWinner));
		var gameInProgress = GameInProgress.Instance;
		var winnerId = gameInProgress.WinnerId;
		var winner = (winnerId == 0 ? gameInProgress.LeftPlayer : gameInProgress.RightPlayer);
        leftPlayerPortrait.sprite = gameInProgress.LeftPlayer.portrait;
        rightPlayerPortrait.sprite = gameInProgress.RightPlayer.portrait;
        Debug.Log(winnerId);
		Debug.Log(winner);

		if( winnerId == 0 ) {
			audioHandler.PlayRandomFromList(EventSource.LEFT, winner.taunts);
            leftName.text = winner.characterName;
			leftMessage.SetActive(true);
			rightMessage.SetActive(false);
			leftPlayerPortrait.flipY = false;
            rightPlayerPortrait.flipY = true;
        }
		else {
            audioHandler.PlayRandomFromList(EventSource.RIGHT, winner.taunts);
            rightName.text = winner.characterName;
			leftMessage.SetActive(false);
			rightMessage.SetActive(true);
            leftPlayerPortrait.flipY = true;
            rightPlayerPortrait.flipY = false;
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

	private void UpdateSelection() {
		CurrentSelection = _eventSystem.currentSelectedGameObject;
		cursor.transform.position = new Vector2(
			cursor.transform.position.x,
			CurrentSelection.transform.position.y
		);
	}
}