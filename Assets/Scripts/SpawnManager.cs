using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer = null;

    [SerializeField]
    private GameObject _enemyPrefab = null;

    [SerializeField]
    private GameObject[] powerups = null;

    [SerializeField]
    private GameObject[] rarePowerups = null;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        ListenToEvents();
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSecondsRealtime(3.0f);

        while (!_stopSpawning)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, ReturnStartPos(), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSecondsRealtime(5f);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSecondsRealtime(3.0f);

        while (!_stopSpawning)
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
    }

    private void PlayerDied()
    {
        _stopSpawning = true;
    }
}
