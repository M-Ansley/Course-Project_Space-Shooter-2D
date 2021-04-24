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

    public SpawnManager spawnManager;
    public UIManager uiManager;

    public bool waveComplete = false;

    private void Start()
    {
        Cursor.visible = false;
        ListenToEvents();

        foreach (Wave wave in waves)
        {
            wavesQueue.Enqueue(wave);
        }
        
    }

    public void BeginGame()
    {
        StartCoroutine(LoadWave(2f));
    }

    private void Update()
    {
        Toggles();
        if (spawnManager.enemiesRemaining == 0 && spawnManager.enemiesToSpawn == 0)
        {
            waveComplete = true;
        }
    }

    private IEnumerator LoadWave(float delay = 0f)
    {
        yield return new WaitForSecondsRealtime(delay);

        if (wavesQueue.Count > 0)
        {
            Wave currentWave = wavesQueue.Dequeue();
            // Debug.Log("Wave " + currentWave.waveNum);
            StartCoroutine(uiManager.DisplayWave(currentWave.waveNum, 2f));
            yield return new WaitForSecondsRealtime(2f);
            spawnManager.StartSpawning(currentWave.enemyPrefabs, currentWave.numOfEnemies, currentWave.delayBetweenEnemies);
            waveComplete = false;            
            while (!waveComplete)
            {
                yield return null;
            }
            Debug.Log("Wave complete");
            StartCoroutine(LoadWave(2f));
        }
        else
        {
            Debug.Log("No waves remain");
        }

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
