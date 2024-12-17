using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour {
	[SerializeField]
	private EventSystem _eventSystem;

	[SerializeField]
	private GameObject cursor;

	private GameObject _currentSelection;
	
	[SerializeField]
	private TMPro.TMP_Text _txtOnScreenControls;

	private GameObject CurrentSelection {
		get => _currentSelection;
		set {
			if( value == null )
				_eventSystem.SetSelectedGameObject(_currentSelection);
			else
				_currentSelection = value;
		}
	}

	private void Start() {
		UpdateShowOnScreenControlsText();
	}

	void Update() {
		UpdateSelection();
	}

	public void HandleOnScreenControls() {
        if( Time.timeSinceLevelLoad <= 1)
			return;
		var gameInProgress = GameInProgress.Instance;
		gameInProgress.ShowOnScreenControls = !gameInProgress.ShowOnScreenControls;
		UpdateShowOnScreenControlsText();
	}

	public void HandleMainMenu() {
        if( Time.timeSinceLevelLoad <= 1)
			return;
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Back");
		GameInProgress.Instance.LoadScene("TitleScreen");
	}

	private void UpdateSelection() {
		CurrentSelection = _eventSystem.currentSelectedGameObject;
		cursor.transform.position = new Vector2(
			cursor.transform.position.x,
			CurrentSelection.transform.position.y
		);
	}

	private void UpdateShowOnScreenControlsText() {
		var gameInProgress = GameInProgress.Instance;
		_txtOnScreenControls.text = gameInProgress.ShowOnScreenControls ? "ON" : "OFF";
	}
}