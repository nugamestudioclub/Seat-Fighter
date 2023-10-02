using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	[SerializeField] private EventSystem eventsystem;

	[SerializeField] private GameObject cursor;

	private GameObject _currentSelection;

	private GameObject CurrentSelection {
		get => _currentSelection;
		set {
			if( value == null )
				eventsystem.SetSelectedGameObject(_currentSelection);
			else
				_currentSelection = value;
		}
	}

	public void Set1Player() {
		GameInProgress.Instance.PlayerCount = 1;
		SceneManager.LoadScene("CharSel");
	}

	public void Set2Player() {
		GameInProgress.Instance.PlayerCount = 2;
		SceneManager.LoadScene("CharSel");
	}

	public void Controls() {
		SceneManager.LoadScene("Controls");
	}

	public void Credits() {
		SceneManager.LoadScene("Credits");
	}

	public void Exit() {
		Application.Quit();
	}

	public void Update() {
		CurrentSelection = eventsystem.currentSelectedGameObject;
		cursor.transform.position = new Vector2(
			cursor.transform.position.x,
			CurrentSelection.transform.position.y
		);
	}
}