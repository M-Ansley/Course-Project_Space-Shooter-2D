using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    [Tooltip("How long it takes to get from start to end pos")]
    [SerializeField] private float _moveDuration = 4f;

    private float _speed = 0;
    [SerializeField] private float _trueSpeed = 4f;

    [Tooltip("Respawn Pos (offscreen)")]
    [SerializeField] private Vector3 startPos;
    [Tooltip("End Pos before Respawning (offscreen)")]
    [SerializeField] private Vector3 endPos;

    private void Start()
    {
        ListenToEvents();
    }

    private void AsteroidDestroyed()
    {
        StartCoroutine(GainSpeed(3f));
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            if (transform.position.y <= endPos.y)
            {
                transform.position = startPos;
            }
            else
            {
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
            }
            yield return null;
        }
    }

    private IEnumerator GainSpeed(float duration)
    {
        float startSpeed =  _speed;
        float elapsedTime = 0;

        while (_speed <= _trueSpeed)
        {
            _speed = Mathf.Lerp(startSpeed, _trueSpeed, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ListenToEvents()
    {
        GameEvents.current.asteroidDestroyed += AsteroidDestroyed;
    }
}
