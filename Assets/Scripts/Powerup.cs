using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _multiplier = 3;
    private float _yMinVal = -6;

    public enum PowerupType
    {
        Triple_Shot,
        Speed,
        Shield,
        Ammo,
        Health,
        Shock,
        Damage
    }

    public PowerupType powerupType;

    private GameObject player;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.C))
        {
            if (player != null)
            {
                float step = (_multiplier * 1.5f) * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
            }
        }
        else
        {
            transform.Translate(Vector3.down * _multiplier * Time.deltaTime);
        }

        if (transform.position.y < _yMinVal)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PowerupLogic();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("EnemyLaser"))
        {
            FindObjectOfType<AudioManager>().Play("Explosion");
            Destroy(gameObject);
        }
    }

    private void PowerupLogic()
    {
        switch (powerupType)
        {
            case PowerupType.Triple_Shot:
                GameEvents.current.PowerupCollected("Triple_Shot");
                break;
            case PowerupType.Speed:
                GameEvents.current.PowerupCollected("Speed");
                break;
            case PowerupType.Shield:
                GameEvents.current.PowerupCollected("Shield");
                break;
            case PowerupType.Ammo:
                GameEvents.current.PowerupCollected("Ammo");
                break;
            case PowerupType.Health:
                GameEvents.current.PowerupCollected("Health");
                break;
            case PowerupType.Shock:
                GameEvents.current.PowerupCollected("Shock");
                break;
            case PowerupType.Damage:
                GameEvents.current.PowerupCollected("Damage");
                break;
            default:
                Debug.LogWarning("Case not recognised");
                break;
        }
    }
}
