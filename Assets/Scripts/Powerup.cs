using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    private float _multiplier = 3;
    private float _yMinVal = -6;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _multiplier * Time.deltaTime);
        if (transform.position.y < _yMinVal)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameEvents.current.PowerupCollected("Triple_Shot");
            Destroy(gameObject);
        }
    }
}
