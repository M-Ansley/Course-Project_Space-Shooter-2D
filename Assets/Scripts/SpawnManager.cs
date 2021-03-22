using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }
        
    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            Instantiate(_enemyPrefab);

            yield return new WaitForSecondsRealtime(5f);
        }
    }
}
