using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static VariableContainer;

public class Enemy : MonoBehaviour
{
    public EnemyType enemyType;

    [SerializeField]
    private BoxCollider2D _collider = null;

    [SerializeField]
    private GameObject _laserPrefab = null;

    [SerializeField]
    private GameObject _beamPrefab = null;

    private GameObject _beam = null;

    [SerializeField]
    private bool _hasShield = false;

    [SerializeField]
    private GameObject _shield = null;

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

    private Coroutine _fireCoroutine;
    private Coroutine _beamCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        RespawnAtTop();
        _fireCoroutine = StartCoroutine(FireCoroutine());
        StartCoroutine(SideToSide(Random.Range(10, 20)));
    }

    // private void



    // Update is called once per frame
    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Default:
                transform.Translate(Vector3.down * _multiplier * Time.deltaTime);
                if (_sideToSide)
                    SideToSide();
                break;

            case EnemyType.Beamer:
                if (transform.position.x < _minXVal || transform.position.z > _maxXVal)
                {
                    _moveLeft = !_moveLeft;
                }

                if (_moveLeft)
                {
                    transform.Translate(new Vector3(-0.5f, -0.5f, 0) * _multiplier * Time.deltaTime);

                }
                else
                {
                    transform.Translate(new Vector3(0.5f, -0.5f, 0) * _multiplier * Time.deltaTime);
                }
                break;

            default:
                break;
        }


        if (_alive && transform.position.y < _yMinVal)
        {
            RespawnAtTop();
            if (enemyType == EnemyType.Beamer)
            {
                _moveLeft = !_moveLeft; // reverse their direction
            }
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
            yield return new WaitForSecondsRealtime(1.5f);
            if (enemyType != EnemyType.Beamer)
            {
                FireLaser();
            }
            else
            {
                _beamCoroutine = StartCoroutine(FireBeam(1.5f));

            }
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

    // For BEAMER enemy types
    private IEnumerator FireBeam(float duration)
    {
        Vector3 positionOffset = new Vector3(transform.position.x, transform.position.y - 21f, transform.position.z);
        _beam = Instantiate(_beamPrefab, positionOffset, Quaternion.identity);
        _beam.transform.parent = this.gameObject.transform;
        //FindObjectOfType<AudioManager>().Play("Beam");
        yield return new WaitForSecondsRealtime(duration);
        if (_beam != null)
        {
            Destroy(_beam);
        }
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

                if (_hasShield)
                {
                    FindObjectOfType<AudioManager>().Play("Explosion");
                    _shield.SetActive(false);
                    _hasShield = false;
                    return;
                }
                StartCoroutine(DestroySelf());
            }
            else if (other.CompareTag("PlayerLaser"))
            {
                Destroy(other.gameObject);
                GameEvents.current.PlayerKill(_scoreForKilling);
                if (_hasShield)
                {
                    FindObjectOfType<AudioManager>().Play("Explosion");
                    _shield.SetActive(false);
                    _hasShield = false;
                    return;
                }
                StartCoroutine(DestroySelf());
            }
            else if (other.CompareTag("PlayerShockWave"))  // Nothing stops the shockwave, not even a shield
            {
                GameEvents.current.PlayerKill(_scoreForKilling);
                StartCoroutine(DestroySelf());
            }
            else if (other.CompareTag("PlayerHomingMissile"))
            {
                if (other.GetComponent<HomingProjectile>() != null)
                {
                    other.GetComponent<HomingProjectile>().Explode();
                }

                GameEvents.current.PlayerKill(_scoreForKilling);
                if (_hasShield)
                {
                    FindObjectOfType<AudioManager>().Play("Explosion");
                    _shield.SetActive(false);
                    _hasShield = false;
                    return;
                }
                StartCoroutine(DestroySelf());
            }

        }
    }

    private IEnumerator DestroySelf()
    {
        FindObjectOfType<AudioManager>().Play("Explosion");
        animator.SetTrigger("OnEnemyDeath");
        GameEvents.current.EnemyDestroyed();
        if (_fireCoroutine != null)
            StopCoroutine(_fireCoroutine);
        if (_beamCoroutine != null)
            StopCoroutine(_beamCoroutine);
        if (_beam != null)
        {
            Destroy(_beam);
        }
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
