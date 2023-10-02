using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private EventSystem eventsystem;
    [SerializeField] private GameObject cursor;
    
    public void Update()
    {
        cursor.transform.position = new Vector2(cursor.transform.position.x,
            eventsystem.currentSelectedGameObject.transform.position.y);
    }
    
    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
    public void PlayAgain() {
        SceneManager.LoadScene(1);
    }
    
    public void Exit()
    {
        Application.Quit();
    }
}
