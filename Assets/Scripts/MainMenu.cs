using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	[SerializeField] private EventSystem eventsystem;
	[SerializeField] private GameObject cursor;
	public void Set1Player() {
		GameInProgress.Instance.PlayerCount = 1;
		SceneManager.LoadScene(0);
	}

	public void Set2Player() {
		GameInProgress.Instance.PlayerCount = 2;
		SceneManager.LoadScene(1);
	}

	public void Controls()
	{
		SceneManager.LoadScene(4);
	}
	
	public void Credits()
	{
		SceneManager.LoadScene(3);
	}
	
	public void Exit()
	{
		Application.Quit();
	}
	
	public void Update()
	{
		cursor.transform.position = new Vector2(cursor.transform.position.x,
			eventsystem.currentSelectedGameObject.transform.position.y);
	}
	
}