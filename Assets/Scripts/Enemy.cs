using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VariableContainer;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _collider = null;

    [SerializeField]
    private GameObject _laserPrefab = null;

    [SerializeField]
    private Animator animator = null;

    [SerializeField]
    private int _scoreForKilling = 50;

    [SerializeField]
    private float _multiplier = 4f;

    private float _startingYVal = 7;
    private float _yMinVal = -6;

    private float _minXVal = -10;
    private float _maxXVal = 10;

    private bool _alive = true;

    private bool _sideToSide = false;
    private bool _moveLeft = true;

    Vector3 maxXPos;
    Vector3 minXPos;    

    // Start is called before the first frame update
    void Start()
    {
        RespawnAtTop();
        StartCoroutine(FireCoroutine());
        StartCoroutine(SideToSide(Random.Range(10, 20)));
    }

    // private void



    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * _multiplier * Time.deltaTime);

        if (_alive && transform.position.y < _yMinVal)
        {
            RespawnAtTop();
        }

        if (_sideToSide)
        {
            SideToSide();
        }
    }

    private void RespawnAtTop()
    {
        maxXPos = new Vector3(transform.position.x + 1, transform.position.y, transform.position.z);
        minXPos = new Vector3(transform.position.x - 1, transform.position.y, transform.position.z);

        float randomXPos = Random.Range(_minXVal, _maxXVal);
        transform.position = new Vector3(randomXPos, _startingYVal, 0);
    }

    private IEnumerator FireCoroutine()
    {
        while (true)
        {
            FireLaser();
            yield return new WaitForSecondsRealtime(Random.Range(3f, 7f)); // wait for 3-7 seconds
        }
    }

    float elapsedTime = 0;

    private void SideToSide()
    {
        if (_moveLeft)
        {
            if (transform.position.x > maxXPos.x)
            {
                _moveLeft = false;
            }
            else
            {
                transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y, transform.position.z);
            }
        }

        if (!_moveLeft)
        {
            if (transform.position.x < minXPos.x)
            {
                _moveLeft = true;
            }
            else
            {
                transform.position = new Vector3(transform.position.x - 0.02f, transform.position.y, transform.position.z);
            }
        }
    }

    private IEnumerator SideToSide(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        _sideToSide = true;               
    }

    // PingPongLerp movement
        //Vector3 maxXPos = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
        //Vector3 minXPos = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);

        //float elapsedTime = 0;

        //while (true)
        //{
        //    transform.position = Vector3.Lerp(minXPos, maxXPos, Mathf.PingPong(elapsedTime, 0.5f));
        //    elapsedTime += Time.deltaTime;
        //    yield return null;
        //}

    private void FireLaser()
    {
        Vector3 positionOffset = new Vector3(transform.position.x, transform.position.y - 1.902f, transform.position.z);
        GameObject laser = Instantiate(_laserPrefab, positionOffset, Quaternion.identity);
        laser.transform.Rotate(0, 0, -180);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_alive)
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
                StartCoroutine(DestroySelf());
            }
            else if (other.CompareTag("PlayerLaser"))
            {
                Destroy(other.gameObject);
                GameEvents.current.PlayerKill(_scoreForKilling);
                StartCoroutine(DestroySelf());
            }
            else if (other.CompareTag("PlayerShockWave"))
            {
                GameEvents.current.PlayerKill(_scoreForKilling);
                StartCoroutine(DestroySelf());
            }

        }
    }

    private IEnumerator DestroySelf()
    {
        FindObjectOfType<AudioManager>().Play("Explosion");
        animator.SetTrigger("OnEnemyDeath");
        GameEvents.current.EnemyDestroyed();
        _alive = false;
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Destroyed")) // i.e. wait until the animation before this has played
        {
            yield return null;
        }
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
