﻿using System;
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

}
