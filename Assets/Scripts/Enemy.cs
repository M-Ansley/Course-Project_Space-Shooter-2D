﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _scoreForKilling = 50;

    [SerializeField]
    private float _multiplier = 4f;

    private float _startingYVal = 7;
    private float _yMinVal = -6;

    private float _minXVal = -10;
    private float _maxXVal = 10;

    // Start is called before the first frame update
    void Start()
    {
        RespawnAtTop();
    }

   // private void
        
    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _multiplier * Time.deltaTime);

        if (transform.position.y < _yMinVal)
        {
            RespawnAtTop();
        }
    }

    private void RespawnAtTop()
    {
        float randomXPos = Random.Range(_minXVal, _maxXVal);
        transform.position = new Vector3(randomXPos, _startingYVal, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if other is Player
        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                print(gameObject.name);
                other.GetComponent<Player>().DamagePlayer();
            }
            DestroySelf();
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            GameEvents.current.PlayerKill(_scoreForKilling);
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        FindObjectOfType<AudioManager>().Play("Explosion");
        GameEvents.current.EnemyDestroyed();
        Destroy(this.gameObject);
    }
}





// UPDATE MODEL
// Update is called once per frame
//void Update()
//{
//    // Move down at 4 meters per second
//    transform.Translate(Vector3.down * _multiplier * Time.deltaTime);

//    // if bottom of screen
//    // respawn at top with a new random x position
//    if (transform.position.y < _yMinVal)
//    {
//        RespawnAtTop();
//    }
//}

//private void RespawnAtTop()
//{
//    float randomXPos = Random.RandomRange(-11, 11);
//    transform.position = new Vector3(randomXPos, _startingYVal, 0);
//}
