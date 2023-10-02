using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInProgress : MonoBehaviour {
	private static GameInProgress _instance;

	public static GameInProgress Instance => _instance;

	[field: SerializeField]
	public int WinnerId { get; set; }

	[field: SerializeField]
	public bool ShowOnScreenControls { get; set; }

	[field: SerializeField]
	public int PlayerCount { get; set; } = 1;

	[field: SerializeField]
	public PlayerOptionsConfig AllPlayers { get; set; }

	[field: SerializeField]
	public PlayerConfig LeftPlayer { get; set; }

	[field: SerializeField]
	public PlayerConfig RightPlayer { get; set; }

	public InputController LeftInput { get; set; }

	public InputController RightInput { get; set; }

	void Awake() {
		if( _instance == null ) {
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}

	public void LoadScene(string name) {
		if( LeftInput != null )
			LeftInput.IsActive = false;
		if( RightInput != null )
			RightInput.IsActive = false;
		StartCoroutine(DoLoadScene(name));
		if( LeftInput != null )
			LeftInput.IsActive = true;
		if( RightInput != null )
			RightInput.IsActive = true;
	}

	private IEnumerator DoLoadScene(string name) {
		SceneManager.LoadScene(name);
		yield return new WaitForSeconds(0.5f);
	}
}