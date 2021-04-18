using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _zRotationVal = 0.75f;

    [SerializeField]
    private GameObject _explosionPrefab = null;

    private SpawnManager _spawnManager = null;

    private bool _alive = true;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(0f, 0f, _zRotationVal, Space.Self);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_alive)
        {


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
                //GameEvents.current.PlayerKill(_scoreForKilling);
                StartCoroutine(DestroySelf());
            }
            else if (other.CompareTag("PlayerShockWave"))
            {
                StartCoroutine(DestroySelf());
            }
        }

    }

    private IEnumerator DestroySelf()
    {
        FindObjectOfType<AudioManager>().Play("Explosion");
        GameObject explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        GameEvents.current.EnemyDestroyed();
        _alive = false;
        Destroy(explosion, 2f);
        _spawnManager.StartSpawning();
        Destroy(this.gameObject, 0.4f);
        yield return null;
        //yield return new WaitForSecondsRealtime(2.30f);
    }
}

