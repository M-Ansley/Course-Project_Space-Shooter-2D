using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // Player inherits or extends monobehaviour. Lets us drag and drop them onto gameobjects, in order to control them.
{
    // public or private. If private, only this class knows it exists.
    // data types. Key data types (int, float, bool, string)
    [Header("Health")]
    [SerializeField]
    private int _lives = 3;

    [Header("Movement")]
    [SerializeField]
    private float _speed = 3.5f;
    private float _speedMultiplier = 1.5f;


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
    private GameObject _tripleShotPrefab = null;

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

    void Start()
    {
        _shieldVisual.SetActive(false);
        _speedVisual.SetActive(false);
        ListenToEvents();
        transform.position = new Vector3(0, 0, 0);

        _nums = new List<int>(_engines.Length);
        for (int i = 0; i < _engines.Length; i++)
        {
            _nums.Add(i);
        }


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
        FindObjectOfType<AudioManager>().Play("Laser");
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
            FindObjectOfType<AudioManager>().Play("Explosion");
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
        FindObjectOfType<AudioManager>().Play("Power-Up");
        switch (powerupName)
        {
            case "Triple_Shot":
                TripleShotActive();
                break;
            case "Speed":
                SpeedActive();
                break;
            case "Shield":
                ShieldActive();
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
        _speedActive = true;
        _speed *= _speedMultiplier; // double the movement speed
        _speedVisual.SetActive(true);
        _audioSource.pitch = 1.7f;
        StartCoroutine(SpeedPowerDownRoutine(5f));
    }

    IEnumerator SpeedPowerDownRoutine(float delay)
    {
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
            _shieldVisual.transform.localScale = Vector3.Lerp(startingScale, desiredScale, (elapsedTime/duration));
            elapsedTime += Time.deltaTime;
            yield return null;
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
