using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer = null;

    [SerializeField]
    private GameObject _enemyPrefab = null;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        ListenToEvents();
        StartCoroutine(SpawnRoutine());
    }
        
    IEnumerator SpawnRoutine()
    {
        while (!_stopSpawning)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, gameObject.transform.position, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSecondsRealtime(5f);
        }
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
