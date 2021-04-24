using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableContainer
{
    public enum EnemyType
    {
        Default
    }

    [System.Serializable]
    public struct Wave
    {
        public int waveNum;
        public bool canDodge;
        public int numOfEnemies;
        public GameObject[] enemyPrefabs;
    }


}
