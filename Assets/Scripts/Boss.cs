using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public class Boss : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer = null;
    private Animator _animator = null;

    [Header("Setup")]
    private Color defaultColor;
    [Tooltip("Boss to move to this position before firing")]
    [SerializeField] private Vector3 endPos;
    [SerializeField] private int _health = 200;
    private bool _alive = true;

    [Header("Weapons")]
    [SerializeField] private ArcFire _leftCannon;
    [SerializeField] private ArcFire _rightCannon;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = _spriteRenderer.color;
        StartCoroutine(Prepare());
    }

    private IEnumerator Prepare(float moveTime = 5f)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0;

        while (transform.position != endPos)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(Attack());


    }

    // ATTACK
    private IEnumerator Attack()
    {
        RandomAttack();
        while (_alive)
        {
            StartCoroutine(MoveToPos(ReturnLeftOrRightPos()));
            yield return new WaitForSecondsRealtime(20f); // every 15 seconds, launch another attack
        }
    }

    private Vector3 ReturnLeftOrRightPos()
    {
        Vector3 newPos = transform.position + new Vector3(3, 0, 0);
        float randomNum = Random.Range(0.0f, 1.0f);
        Debug.Log(randomNum);
        if (randomNum >= 0.5f)
        {
            newPos = transform.position + new Vector3(-3, 0, 0);
        }
        return newPos;
    }

    private IEnumerator MoveToPos(Vector3 movePos, float duration = 6f)
    {
        Vector3 startPos = transform.position;

        float elapsedTime = 0;

        while (transform.position != movePos)
        {
            transform.position = Vector3.Lerp(startPos, movePos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Position 1 reached");

        elapsedTime = 0;

        Vector3 currentPos = transform.position;

        while (transform.position != startPos)
        {
            transform.position = Vector3.Lerp(currentPos, startPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Position 2 reached");
    }

    private int _bossMoves = 1;

    private void RandomAttack()
    {
        int randomNum = Random.Range(1, _bossMoves);
        switch (randomNum)
        {
            case 1:
                _leftCannon.Stop();
                _rightCannon.Stop();
                StartCoroutine(_leftCannon.StartFireRoutine(10f));
                StartCoroutine(_rightCannon.StartFireRoutine(10f));
                break;
            default:
                break;
        }
    }



    // DAMAGE
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
                    other.GetComponent<Player>().DamagePlayer();
                    FindObjectOfType<AudioManager>().Play("Explosion");
                }
            }
            else if (other.CompareTag("PlayerLaser"))
            {
                Destroy(other.gameObject);
                DamageSelf(1);
            }
            else if (other.CompareTag("PlayerShockWave"))  // Nothing stops the shockwave, not even a shield
            {
                DamageSelf(20);
            }
            else if (other.CompareTag("PlayerHomingMissile"))
            {
                if (other.GetComponent<HomingProjectile>() != null)
                {
                    other.GetComponent<HomingProjectile>().Explode();
                }
                DamageSelf(10);
            }
        }
    }

    Coroutine damageCoroutine;

    private void DamageSelf(int amount)
    {
        if (_health - amount > 0)
        {
            _health -= amount;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
            damageCoroutine = StartCoroutine(DamageCoroutine(amount));
        }
        else
        {
            _health = 0;
            StartCoroutine(DestroySelf());
        }
    }

    private IEnumerator DamageCoroutine(int damage)
    {
        _spriteRenderer.color = defaultColor;
        yield return new WaitForSecondsRealtime(0.05f);
        _spriteRenderer.color = Color.red;
        if (damage >= 10)
        {
            FindObjectOfType<AudioManager>().Play("Explosion");
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("MinorDamage");
        }
        yield return new WaitForSecondsRealtime(0.5f);
        _spriteRenderer.color = defaultColor;
    }

    private void StopAllWeapons()
    {
        _leftCannon.Stop();
        _rightCannon.Stop();
    }

    private IEnumerator DestroySelf()
    {
        FindObjectOfType<AudioManager>().Play("Explosion");
        _animator.SetTrigger("OnEnemyDeath");
        // GameEvents.current.EnemyDestroyed();
        GameEvents.current.BossAlive(false);

        StopAllWeapons();
        Destroy(_leftCannon.gameObject);
        Destroy(_rightCannon.gameObject);
        //if (_fireCoroutine != null)
        //    StopCoroutine(_fireCoroutine);
        //if (_beamCoroutine != null)
        //    StopCoroutine(_beamCoroutine);
        //if (_beam != null)
        //{
        //    Destroy(_beam);
        //}
        _alive = false;
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Enemy_Destroyed")) // i.e. wait until the animation before this has played
        {
            yield return null;
        }
        Destroy(this.gameObject);
        yield return null;
    }

}
