using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;
using FMOD;

public class MainMenu : MonoBehaviour {
	[SerializeField]
	private EventSystem _eventSystem;

	[SerializeField]
	private GameObject cursor;

	private GameObject _currentSelection;

	private GameObject CurrentSelection {
		get => _currentSelection;
		set {
			if( value == null )
				_eventSystem.SetSelectedGameObject(_currentSelection);
			else
				_currentSelection = value;
		}
	}

	void Update() {
		UpdateSelection();
	}

	public void Set1Player() {
		GameInProgress.Instance.PlayerCount = 1;
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Select");
		GameInProgress.Instance.LoadScene("CharacterSelect");
	}

	public void Set2Player() {
		GameInProgress.Instance.PlayerCount = 2;
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Select");
		GameInProgress.Instance.LoadScene("CharacterSelect");
	}

	public void Controls() {
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Select");
		GameInProgress.Instance.LoadScene("Controls");
	}
	
	public void Tutorial() {
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Select");
		GameInProgress.Instance.LoadScene("Tutorial");
	}
	
	public void UiExplaination() {
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Select");
		GameInProgress.Instance.LoadScene("UiExplaination");
	}
	

	public void Credits() {
		FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Select");
		GameInProgress.Instance.LoadScene("Credits");
	}

	public void Exit() {
		Application.Quit();
	}

	private void UpdateSelection() {
		if (CurrentSelection != _eventSystem.currentSelectedGameObject) {
			if (CurrentSelection != null) {
				FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/UI/Move Cursor");
			}
		}
		CurrentSelection = _eventSystem.currentSelectedGameObject;
		cursor.transform.position = new Vector2(
			CurrentSelection.transform.position.x,
			CurrentSelection.transform.position.y
			
		);
	}
}