using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableContainer
{
    public enum EnemyType
    {
        Default,
        Beamer
    }

    [System.Serializable]
    public struct Wave
    {
        public int waveNum;
        public bool canDodge;
        public float delayBetweenEnemies;
        public int numOfEnemies;
        public GameObject[] enemyPrefabs;
    }


}
