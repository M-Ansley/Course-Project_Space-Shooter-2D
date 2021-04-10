using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Vector3 originalCameraPos;

    [SerializeField]
    private float _shakeIntensity = 0.01f;

    [SerializeField]
    private Camera _mainCamera = null;

    Coroutine shakeCoroutine;

    private void Start()
    {
        originalCameraPos = _mainCamera.transform.position;
       // StartCoroutine(ShakeCameraCoroutine(5f));
    }

    public void TriggerShake(float intensity, float duration)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCameraCoroutine(intensity, duration));
    }

    IEnumerator ShakeCameraCoroutine(float intensity, float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            ShakeCamera(intensity);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(RestoreOriginalPosition(0.5f));
    }

    private void ShakeCamera(float intensity)
    {
        float quakeAmt = Random.value * intensity * 2 - intensity;
        Vector3 previousPos = _mainCamera.transform.position;
        previousPos.y += quakeAmt;
        previousPos.x += quakeAmt;
        _mainCamera.transform.position = previousPos;
    }

    IEnumerator RestoreOriginalPosition(float duration)
    {
        Vector3 startPos = _mainCamera.transform.position;
        float elapsedTime = 0f;

        while (_mainCamera.transform.position != originalCameraPos)
        {
            _mainCamera.transform.position = Vector3.Lerp(startPos, originalCameraPos, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }



    // **********************************************************************
    // EVENTS
    //private void ListenToEvents()
    //{
    //    GameEvents.current.enemyDestroyed += TriggerShake(4f, 32f);
    //}

}
