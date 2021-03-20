using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _multiplier = 8;

    [SerializeField]
    private float maxYVal = 8;



    private void Start()
    {
        StartCoroutine(MoveUntil());
    }

    IEnumerator MoveUntil()
    {
        while (transform.position.y < maxYVal)
        {
            transform.Translate(Vector3.up * _multiplier * Time.deltaTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}





// Lifespan model:
//[SerializeField]
//private float _lifeSpan = 6;

//float elapsedTime = 0;
//        while (elapsedTime<_lifeSpan)
//        {
//            transform.Translate(Vector3.up* _multiplier * Time.deltaTime);
//elapsedTime += Time.deltaTime;
//            yield return null;
//        }