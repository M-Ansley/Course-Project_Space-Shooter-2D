using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _multiplier = 4f;

    private float _startingYVal = 7;
    private float _yMinVal = -6;

    private float _minXVal = -11;
    private float _maxXVal = 11;

    // Start is called before the first frame update
    void Start()
    {
        RespawnAtTop();
    }

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
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }        
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
