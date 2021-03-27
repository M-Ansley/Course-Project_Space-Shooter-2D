using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    public enum EnemyType  {Red, Blue, Yelow}

    public EnemyType enemyType;

    private void Start()
    {
        EnemyTypeSetup();
    }

    private void EnemyTypeSetup()
    {
        switch (enemyType)
        {
            case EnemyType.Blue:
                // Change sprite colour to blue
                // Start blue enemy behaviour coroutine
                break;
            case EnemyType.Red:
                // Change sprite colour to red
                // Start red enemy behaviour coroutine
                break;
            case EnemyType.Yelow:
                // Change sprite colour to yellow
                // Start red enemy behaviour coroutine
                break;
            default:
                break;
        }
    }
}
