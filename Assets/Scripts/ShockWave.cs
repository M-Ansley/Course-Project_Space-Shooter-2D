using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    [SerializeField]
    private float _duration = 3f;

    [SerializeField]
    private Vector3 _endScale;

    private void Start()
    {
        StartCoroutine(Expand());
    }

    private IEnumerator Expand()
    {
        Vector3 startScale = gameObject.transform.localScale;

        float elapsedTime = 0;

        while (transform.localScale != _endScale)
        {
            transform.localScale = Vector3.Lerp(startScale, _endScale, (elapsedTime / _duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
