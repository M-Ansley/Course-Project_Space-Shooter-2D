using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    private bool _gameRunning = true;

    private void Start()
    {
        Cursor.visible = false;
        ListenToEvents();
    }

    private void Update()
    {
        Toggles();
    }

    private void Toggles()
    {
        if (Input.GetKeyDown(KeyCode.R) && !_gameRunning)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void GameOver()
    {
        _gameRunning = false;
    }

    private void ListenToEvents()
    {
        GameEvents.current.playerDied += GameOver;
    }
}
