using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

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

}
