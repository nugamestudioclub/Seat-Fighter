using System;
using System.Collections.Generic;
using UnityEngine;

public class GameInProgress : MonoBehaviour {
	private static GameInProgress _instance;

	public static GameInProgress Instance => _instance;

	[field: SerializeField]
	public int WinnerId { get; set; }

	[field: SerializeField]
	public bool ShowOnScreenControls { get; set; }

	[field: SerializeField]
	public int PlayerCount { get; set; } = 1;

	public GameConfig Config { get; set; }

	void Awake() {
		if( _instance == null ) {
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}
}