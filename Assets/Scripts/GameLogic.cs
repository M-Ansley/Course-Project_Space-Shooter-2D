using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static VariableContainer;

public class GameLogic : MonoBehaviour
{
    private bool _gameRunning = true;

    public Wave[] waves;

    public Queue<Wave> wavesQueue = new Queue<Wave>();

    private void Start()
    {
        Cursor.visible = false;
        ListenToEvents();

        foreach (Wave wave in waves)
        {
            wavesQueue.Enqueue(wave);
        }
        
    }

    private void Update()
    {
        Toggles();
    }

    //private IEnumerator LoadWave()
    //{
    //    if (wavesQueue.Count > 0)
    //    {

    //    }
    //    else
    //    {
    //        Debug.Log("No waves remain");
    //    }
        
    //}

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
