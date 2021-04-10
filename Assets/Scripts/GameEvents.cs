﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    [SerializeField]
    private ScreenShake _screenShake;

    [SerializeField]
    private float _screenShakeDuration = 0.25f;

    [SerializeField]
    private float _screenShakeIntensity = 0.05f;

    private void Awake()
    {
        current = this;
    }

    public event Action playerDamaged;

    public void PlayerDamaged()
    {
        if (playerDamaged != null)
        {
            playerDamaged();
        }
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
        _screenShake.TriggerShake(_screenShakeIntensity, _screenShakeDuration);
        if (enemyDestroyed != null)
        {
            enemyDestroyed();
        }
    }

    public event Action laserFired;

    public void LaserFired()
    {
        _screenShake.TriggerShake(_screenShakeIntensity, _screenShakeDuration);
        if (laserFired != null)
        {
            laserFired();
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
