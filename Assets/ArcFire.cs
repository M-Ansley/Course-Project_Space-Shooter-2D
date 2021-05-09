using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcFire : MonoBehaviour
{
    [SerializeField] private GameObject _laserPrefab;

    [SerializeField] private GameObject _head;
    public Transform leftTransform;
    public Transform rightTransform;
    public Transform centerTransform;
    [SerializeField] private float turningRate = 30f;
    [SerializeField] private float fireRate = 0.5f;

    bool rotateLeft = false;

    private void Start()
    {
        StartCoroutine(RotateToDefault());
    }

    Coroutine turnCoroutine;
    Coroutine fireCoroutine;

    public IEnumerator StartFireRoutine(float duration = 0)
    {
        Stop();
        turnCoroutine = StartCoroutine(TurnCoroutine());
        fireCoroutine = StartCoroutine(FireLaser());
        yield return new WaitForSeconds(duration);
       // yield return new WaitForSeconds(duration);
       // Stop();
       // StartCoroutine(RotateToDefault());
    }


    private IEnumerator TurnCoroutine()
    {
        Quaternion startRotation = new Quaternion();
        Quaternion endRotation = new Quaternion();

        rotateLeft = !rotateLeft;

        if (rotateLeft)
        {
            startRotation = rightTransform.rotation;
            endRotation = leftTransform.rotation;
        }
        else
        {
            startRotation = leftTransform.rotation;
            endRotation = rightTransform.rotation;
        }
        


        while (transform.rotation != endRotation)
        {
            //transform.rotation = Quaternion.Lerp(startRotation, endRotation, (elapsedTime / 3f));
            // elapsedTime += Time.deltaTime;

            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rightRotationMax, turningRate * Time.deltaTime);
            float singleStep = turningRate * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endRotation, singleStep);

            yield return null;
        }

        StartCoroutine(TurnCoroutine());
    }

    private IEnumerator RotateToDefault()
    {
        while (transform.rotation != centerTransform.rotation)
        {
            float singleStep = turningRate * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, centerTransform.rotation, singleStep);
            yield return null;
        }
    }



    private bool _firing = false;

    private IEnumerator FireLaser()
    {
        _firing = true;
        while (_firing)
        {
            //Vector3 positionOffset = new Vector3(transform.position.x, transform.position.y - 1.902f, transform.position.z);
            GameObject laser = Instantiate(_laserPrefab, _head.transform.position, transform.rotation);
            // laser.transform.Rotate(0, 0, -180);
            yield return new WaitForSecondsRealtime(fireRate);
        }
    }


    public void Stop()
    {
        if (turnCoroutine != null)
        {
            StopCoroutine(turnCoroutine);

        }

        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        _firing = false;
    }
}
