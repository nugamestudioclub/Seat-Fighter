using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlMenu : MonoBehaviour
{
    void Update()
    {
        if (Time.timeSinceLevelLoad > 1)
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}
