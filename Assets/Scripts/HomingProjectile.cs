using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    [SerializeField] private float _multiplier = 2f;
    [SerializeField] private float _duration = 5f; // needs to find target within this time
    [SerializeField] private bool _friendly = true;

    private GameObject _target = null;

    void Start()
    {
        SetTag();
        StartCoroutine(SeekTarget());
    }

    private IEnumerator SeekTarget()
    {
        TryToFindTarget();

        while (_duration > 0)
        {
            if (_target != null)
            {
                float step = _multiplier * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, step);
                FaceTarget();
            }
            else
            {
                TryToFindTarget();
            }

            _duration -= Time.deltaTime;
            yield return null;
        }

        Explode();
    }

    private void SetTag()
    {
        if (!_friendly)
        {
            // Set tag
            if (gameObject.tag != "EnemyHomingMissile")
            {
                gameObject.tag = "EnemyHomingMissile";
            }
        }
        else
        {
            // Set tag
            if (gameObject.tag != "PlayerHomingMissile")
            {
                gameObject.tag = "PlayerHomingMissile";
            }
        }
    }

    private void TryToFindTarget()
    {
        if (_friendly) // find the player
        {        
            if (FindObjectOfType<Enemy>() != null)
            {
                _target = FindObjectOfType<Enemy>().gameObject;
            }
            else // no enemy -- fly off
            {
                gameObject.transform.Translate(new Vector3(0, 1, 0) * _multiplier * Time.deltaTime);
            }
        }
        else // find the player
        {          
            if (FindObjectOfType<Player>() != null)
            {
                _target = FindObjectOfType<Player>().gameObject;
            }
            else // no enemy -- fly off
            {
                gameObject.transform.Translate(new Vector3(0, 1, 0) * _multiplier * Time.deltaTime);
            }
        }

    }

    private void FaceTarget()
    {
        //var targetRotation = Quaternion.LookRotation(_target.transform.position - transform.position);
        //var str = Mathf.Min(0.5f * Time.deltaTime, 1);
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, str);

        Vector3 offset = _target.transform.position - transform.position;

        // Construct a rotation as in the y+ case.
        Quaternion rotation = Quaternion.LookRotation(
                                  new Vector3(0, 0, 1),
                                  offset
                              );

        // Apply a compensating rotation that twists x+ to y+ before the rotation above.
        transform.rotation = rotation * Quaternion.Euler(0, 0, 0);
    }


    public void Explode()
    {
        FindObjectOfType<AudioManager>().Play("Explosion");
        Destroy(gameObject);
    }

}
