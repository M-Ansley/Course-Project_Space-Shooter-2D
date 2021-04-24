using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer = null;

    [SerializeField]
    private GameObject[] _enemyPrefabs = null;

    [SerializeField]
    private GameObject[] powerups = null;

    [SerializeField]
    private GameObject[] rarePowerups = null;

    private Coroutine _powerupCoroutine;
    
    public int enemiesToSpawn = 0;   
    public int enemiesRemaining = 0;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        ListenToEvents();
    }

    public void StartSpawning(GameObject[] enemyPrefabs, int waveEnemies, float delayBetweenEnemies)
    {
        _stopSpawning = false;
        _enemyPrefabs = enemyPrefabs;
        enemiesToSpawn = waveEnemies;

        StartCoroutine(SpawnEnemyRoutine(delayBetweenEnemies));
        if (_powerupCoroutine == null)
        {
            _powerupCoroutine = StartCoroutine(SpawnPowerupRoutine());
        }
    }
    
    IEnumerator SpawnEnemyRoutine(float delayBetweenEnemies)
    {
        yield return new WaitForSecondsRealtime(delayBetweenEnemies);

        while (!_stopSpawning)
        {
            GameObject newEnemy = Instantiate(_enemyPrefabs[Random.Range(0, _enemyPrefabs.Length)], ReturnStartPos(), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;

            enemiesRemaining++;
            enemiesToSpawn--;
            if (enemiesToSpawn == 0)
            {
                _stopSpawning = true;
            }
            yield return new WaitForSecondsRealtime(5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSecondsRealtime(3.0f);

        while (true)
        {
            int randomVal = Random.Range(0, 12);
            if (randomVal <= 1) // spawn a RARE powerup
            {
                Debug.Log("Spawning a rare powerup");
                int randPowerUpNum = Random.Range(0, rarePowerups.Length);
                Instantiate(rarePowerups[randPowerUpNum], ReturnStartPos(), Quaternion.identity);
            }
            else
            {
                int randPowerUpNum = Random.Range(0, powerups.Length);
                Instantiate(powerups[randPowerUpNum], ReturnStartPos(), Quaternion.identity);
            }

            yield return new WaitForSecondsRealtime(Random.Range(3f, 8f));
        }
    }

    private Vector3 ReturnStartPos()
    {
        float randomXPos = Random.Range(-8, 18);
        Vector3 startPos = new Vector3(randomXPos, 7, 0);
        return startPos;
    }

    // *******************************************************************************************
    // EVENTS
    private void ListenToEvents()
    {
        GameEvents.current.playerDied += PlayerDied;
        GameEvents.current.enemyDestroyed += EnemyDestroyed;
    }

    private void EnemyDestroyed()
    {
        enemiesRemaining--;
    }

    private void PlayerDied()
    {
        _stopSpawning = true;
    }
}
