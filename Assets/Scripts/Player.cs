using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // Player inherits or extends monobehaviour. Lets us drag and drop them onto gameobjects, in order to control them.
{
    // public or private. If private, only this class knows it exists.
    // data types. Key data types (int, float, bool, string)

    [Header("Movement")]
    [SerializeField]
    private float _multiplier = 3.5f;

    private float _xClamping = 12;
    private float _yMaxVal = 0;
    private float _yMinVal = -4;

    [Header("Shooting")]
    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = 0;

    [Header("Health")]
    [SerializeField]
    private int _lives = 3;

    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        CalculateMovement();
        Shooting();
    }

    // *******************************************************************************************
    // MOVEMENT

    private void CalculateMovement()
    {
        PlayerMovement();
        Clamping();
    }

    private void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _multiplier * Time.deltaTime);
    }

    private void Clamping()
    {
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, _yMinVal, _yMaxVal), transform.position.z);

        if (transform.position.x >= _xClamping)
        {
            transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -_xClamping)
        {
            transform.position = new Vector3(11, transform.position.y, transform.position.z);
        }
    }

    // *******************************************************************************************
    // SHOOTING

    private void Shooting()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > _nextFire) // A cooldown system
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        _nextFire = Time.time + _fireRate;
        Vector3 positionOffset = new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
        Instantiate(_laserPrefab, positionOffset, Quaternion.identity);
    }

    // *******************************************************************************************
    // HEALTH

    public void DamagePlayer()
    {
        _lives--;
        if (_lives < 1)
        {
            GameEvents.current.PlayerDied();
            Destroy(this.gameObject);
        }
    }
}








// Notes

// Vector3.right is the equivalent of new Vector3(1,0,0).
// Which means moving us 1 Unity unit (a meter) per frame (so roughly 60 meters per second)
// Incorporate real time to get this to happen per second instead
//transform.Translate(Vector3.right * _multiplier * Time.deltaTime);



// Original Y Axis clamping (using if statements)
//if (transform.position.y >= _yCMaxVal)
//{
//    transform.position = new Vector3(transform.position.x, _yCMaxVal, transform.position.z);
//}
//else if (transform.position.y <= _yCMinVal)
//{
//    transform.position = new Vector3(transform.position.x, _yCMinVal, transform.position.z);
//}
