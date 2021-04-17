﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // Player inherits or extends monobehaviour. Lets us drag and drop them onto gameobjects, in order to control them.
{
    // public or private. If private, only this class knows it exists.
    // data types. Key data types (int, float, bool, string)
    private AudioManager _audioManager = null;
    private AmmoDisplay _ammoDisplay = null;

    [Header("Health")]
    [SerializeField]
    private int _lives = 3;

    [Header("Movement")]
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedMultiplier = 2f;
    private float _originalSpeed;


    private float _xClamping = 12;
    private float _yMaxVal = 0;
    private float _yMinVal = -4;

    [Header("Audio")]
    [SerializeField]
    private AudioSource _audioSource = null;

    [Header("Shooting")]
    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _nextFire = 0;

    [SerializeField]
    private int _maxAmmo = 15;
    private int _currentAmmo;

    [SerializeField]
    private GameObject _tripleShotPrefab = null;

    [Header("Thrusters")]
    [SerializeField]
    private float _thrustersMaxCharge = 200;
    public float thrustersCurrentCharge;
    private bool _thrustersOn = false;
    private Coroutine _thrustersCoroutine;
    private Coroutine _thrustersRechargeCoroutine;

    [Header("Speed")]
    [SerializeField]
    private GameObject _speedVisual = null;

    [Header("Shield")]
    [SerializeField]
    private GameObject _shieldVisual = null;

    [Header("Engines")]
    [SerializeField]
    private GameObject[] _engines = null;
    private int _activeThrusterNum = 0;
    [SerializeField]
    private List<int> _nums = null;


    private bool _tripleShotActive = false;
    private bool _speedActive = false;
    private bool _shieldActive = false;

    private void Start()
    {
        ListenToEvents();
        FindObjects();
        VariableSetup();
    }

    private void VariableSetup()
    {
        _shieldVisual.SetActive(false);
        _speedVisual.SetActive(false);
        transform.position = new Vector3(0, 0, 0);
        _originalSpeed = _speed;
        thrustersCurrentCharge = _thrustersMaxCharge;
        _currentAmmo = _maxAmmo;

        _nums = new List<int>(_engines.Length);

        for (int i = 0; i < _engines.Length; i++)
        {
            _nums.Add(i);
        }

        if (_ammoDisplay != null)
            _ammoDisplay.Setup(_maxAmmo);


    }

    private void FindObjects()
    {
        if (FindObjectOfType<AudioManager>() != null)
        {
            _audioManager = FindObjectOfType<AudioManager>();
        }
        else
        {
            Debug.LogWarning("Audio Manager not found in scene");
        }

        if (FindObjectOfType<AmmoDisplay>() != null)
        {
            _ammoDisplay = FindObjectOfType<AmmoDisplay>();
        }
        else
        {
            Debug.LogWarning("Ammo Display not found in scene");
        }
    }


    private void Update()
    {
        CalculateMovement();
        Shooting();
        Thrusters();
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
        transform.Translate(direction * _speed * Time.deltaTime);
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

    private void Thrusters()
    {
        if (!_speedActive)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (_thrustersCoroutine == null)
                {
                    _thrustersCoroutine = StartCoroutine(RunThrusters());
                }
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                StopThrusters();
            }
        }
    }

    private IEnumerator RunThrusters()
    {
        _speed = _originalSpeed * 1.5F;
        _thrustersOn = true;
        _audioSource.pitch = 1.35f;
        if (_thrustersRechargeCoroutine != null)
        {
            StopCoroutine(_thrustersRechargeCoroutine);
            _thrustersRechargeCoroutine = null;
        }
        while (thrustersCurrentCharge > 0)
        {
            thrustersCurrentCharge -= Time.deltaTime * 20;
            yield return null;
        }
        StopThrusters();
    }

    // This needs to reset the thrusters and players speed to exactly how it was before the thrusters started
    private void StopThrusters()
    {
        _thrustersOn = false;
        if (_thrustersCoroutine != null)
        {
            StopCoroutine(_thrustersCoroutine);
        }
        _thrustersCoroutine = null;
        _speed = _originalSpeed;
        _audioSource.pitch = 1f;
        _thrustersRechargeCoroutine = StartCoroutine(RechargeThrusters());
    }

    private IEnumerator RechargeThrusters()
    {
        while (thrustersCurrentCharge < _thrustersMaxCharge)
        {
            thrustersCurrentCharge += Time.deltaTime * 10;
            yield return null;
        }
        thrustersCurrentCharge = _thrustersMaxCharge;
    }


    // *******************************************************************************************
    // SHOOTING

    private void Shooting()
    {
        if (_currentAmmo > 0)
        {
            if (Input.GetButtonDown("Fire1") && Time.time > _nextFire) // A cooldown system
            {
                FireLaser();
            }
        }
    }

    private void FireLaser()
    {
        _currentAmmo--;
        _nextFire = Time.time + _fireRate;
        if (!_tripleShotActive)
        {
            Vector3 positionOffset = new Vector3(transform.position.x, transform.position.y + 1.25f, transform.position.z);
            Instantiate(_laserPrefab, positionOffset, Quaternion.identity);
        }
        else
        {
            Vector3 positionOffset = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            Instantiate(_tripleShotPrefab, positionOffset, Quaternion.identity);

            // GameObject tripleShotInstance = Instantiate(_tripleShotPrefab, positionOffset, Quaternion.identity);
            // Destroy(tripleShotInstance, 2f);
        }

        if (_audioManager != null)
            _audioManager.Play("Laser");

        if (_ammoDisplay != null)
            _ammoDisplay.UpdateAmmo();

        GameEvents.current.LaserFired();
    }

    // *******************************************************************************************
    // HEALTH

    public void DamagePlayer()
    {
        if (_shieldActive)
        {
            _shieldVisual.SetActive(false);
            _shieldActive = false;
            return; // will break us out of the method we're currently in. 
        }

        _lives--;
        if (_lives < 1)
        {
            GameEvents.current.PlayerDied();

            if (_audioManager != null)
                _audioManager.Play("Explosion");

            Destroy(this.gameObject);
        }
        else
        {
            GameEvents.current.PlayerDamaged();
            if (_nums.Count > 1)
            {
                int randomNum = Random.RandomRange(0, _nums.Count);
                print(randomNum);
                _engines[randomNum].SetActive(true);
                _nums.Remove(randomNum);
            }
            else
            {
                _engines[_nums[0]].SetActive(true);
                _nums.Remove(_nums[0]);
            }
        }
    }

    // *******************************************************************************************
    // POWERUPS
    public void PowerupCollected(string powerupName)
    {
        Debug.Log(powerupName + " collected");
        if (_audioManager != null)
            _audioManager.Play("Power-Up");

        switch (powerupName)
        {
            case "Triple_Shot":
                RefillAmmo();
                TripleShotActive();
                break;
            case "Speed":
                SpeedActive();
                break;
            case "Shield":
                ShieldActive();
                break;
            case "Ammo":
                RefillAmmo();
                break;
            default:
                break;
        }
    }

    // TRIPLE SHOT
    private void TripleShotActive()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine(5f));
    }

    IEnumerator TripleShotPowerDownRoutine(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        _tripleShotActive = false;
    }

    // SPEED
    private void SpeedActive()
    {
        print("Speed active");
        StopThrusters();
        // thrustersOn = false;
        // _speed = _originalSpeed;

        _speedActive = true;
        _speed *= _speedMultiplier; // double the movement speed
        print(_speed);
        _speedVisual.SetActive(true);
        _audioSource.pitch = 1.7f;
        StartCoroutine(SpeedPowerDownRoutine(5f));
    }

    IEnumerator SpeedPowerDownRoutine(float delay)
    {
        print("Speed inactive");
        yield return new WaitForSecondsRealtime(delay);
        _speed /= _speedMultiplier;
        _speedVisual.SetActive(false);
        _audioSource.pitch = 1f;
        _speedActive = false;
    }


    private void ShieldActive()
    {
        if (!_shieldActive)
        {
            _shieldActive = true;
            StartCoroutine(ShieldExpand(0.5f));
            _shieldVisual.SetActive(true);
        }
    }

    IEnumerator ShieldExpand(float duration)
    {
        Vector3 desiredScale = _shieldVisual.transform.localScale;
        _shieldVisual.transform.localScale = new Vector3(0, 0, 0);
        Vector3 startingScale = _shieldVisual.transform.localScale;

        float elapsedTime = 0;

        while (_shieldVisual.transform.localScale != desiredScale)
        {
            _shieldVisual.transform.localScale = Vector3.Lerp(startingScale, desiredScale, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }


    private void RefillAmmo()
    {
        _currentAmmo = _maxAmmo;
        if (_ammoDisplay != null)
            _ammoDisplay.Refill();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("EnemyLaser"))
        {
            Destroy(other.gameObject);

            if (_audioManager != null)
                _audioManager.Play("Explosion");

            DamagePlayer();
        }

    }




    // *******************************************************************************************
    // EVENTS

    private void ListenToEvents()
    {
        GameEvents.current.powerupCollected.AddListener(PowerupCollected);
    }

    private void UnlistenToEvents()
    {
        GameEvents.current.powerupCollected.RemoveListener(PowerupCollected);
    }

    // ********************************************************************************************
    // MISC

    private void OnDestroy()
    {
        UnlistenToEvents();
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
