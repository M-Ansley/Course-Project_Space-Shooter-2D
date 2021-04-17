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
        Health
    }

    public PowerupType powerupType;

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
            PowerupLogic();            
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
            default:
                Debug.LogWarning("Case not recognised");
                break;
        }
    }
}
