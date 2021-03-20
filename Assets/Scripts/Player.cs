using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour // Player inherits or extends monobehaviour. Lets us drag and drop them onto gameobjects, in order to control them.
{
    // public or private. If private, only this class knows it exists.
    // data types. Key data types (int, float, bool, string)

    [SerializeField]
    private float _multiplier = 3.5f;

    private float _xClamping = 12;
    private float _yMaxVal = 0;
    private float _yMinVal = -4;

    //public float horizontalInput;

    void Start()
    {
        // Take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
    }
    
    void Update()
    {
        // Vector3.right is the equivalent of new Vector3(1,0,0).
        // Which means moving us 1 Unity unit (a meter) per frame (so roughly 60 meters per second)
        // Incorporate real time to get this to happen per second instead
        //transform.Translate(Vector3.right * _multiplier * Time.deltaTime);

        CalculateMovement();

    }

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






}// Original Y Axis clamping (using if statements)
        //if (transform.position.y >= _yCMaxVal)
        //{
        //    transform.position = new Vector3(transform.position.x, _yCMaxVal, transform.position.z);
        //}
        //else if (transform.position.y <= _yCMinVal)
        //{
        //    transform.position = new Vector3(transform.position.x, _yCMinVal, transform.position.z);
        //}
