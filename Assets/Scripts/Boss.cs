using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Tooltip("Boss to move to this position before firing")] 
    [SerializeField] private Vector3 endPos;

    [SerializeField] private int _health = 200;

    private bool _alive = true;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Prepare());
    }

   private IEnumerator Prepare(float moveTime = 5f)
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0;

        while (transform.position != endPos)
        {
            transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / moveTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Debug.Log("Moved to position. Brace yourself!");
        StartCoroutine(Attack());


    }

    private IEnumerator Attack()
    {
        while (_alive)
        {
            RandomAttack();
            yield return new WaitForSecondsRealtime(15f); // every 15 seconds, launch another attack
        }
    }

    private int _bossMoves = 1;

    private void RandomAttack()
    {
        int randomNum = Random.Range(1, _bossMoves);
        switch(randomNum)
        {
            case 1:
                break;
            default:
                break;
        }
    }

}
