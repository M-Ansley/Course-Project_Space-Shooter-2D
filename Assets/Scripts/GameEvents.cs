using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    [SerializeField]
    private ScreenShake _screenShake;

    private void Awake()
    {
        current = this;
    }

    public event Action playerDied;

    public void PlayerDied()
    {
        if (playerDied != null)
        {
            playerDied();
        }
        Debug.Log("The player has died to death");
    }

    public MyStringEvent powerupCollected;

    [System.Serializable]
    public class MyStringEvent : UnityEvent<string>
    {

    }

    public void PowerupCollected(string powerupName)
    {
        if (powerupCollected != null)
        {
            powerupCollected.Invoke(powerupName);
        }
    }

    public event Action enemyDestroyed;

    public void EnemyDestroyed()
    {
        _screenShake.TriggerShake(0.03f, 0.25f);
        if (enemyDestroyed != null)
        {
            enemyDestroyed();
        }
    }

    [System.Serializable]
    public class MyIntEvent : UnityEvent<int>
    {

    }

    public MyIntEvent playerKill;

    public void PlayerKill(int score)
    {
        if (playerKill != null)
        {
            playerKill.Invoke(score);
        }
    }
   

}
