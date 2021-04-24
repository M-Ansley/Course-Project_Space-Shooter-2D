using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject _gridGameObject = null;

    [SerializeField]
    private GameObject _ammoPrefab = null;

    [SerializeField]
    private List<GameObject> _ammoUnits = new List<GameObject>();

    private int _maxAmmo = 15;
    
    public void Setup(int _playerMaxAmmo)
    {
        _maxAmmo = _playerMaxAmmo;

        for (int i = 0; i < _maxAmmo; i++)
        {
            GameObject ammoUnit = Instantiate(_ammoPrefab, _gridGameObject.transform);
            _ammoUnits.Add(ammoUnit);
        }
    }

    // should only get called when a shot is successfully fired
    public void UpdateAmmo()
    {
        if (_ammoUnits.Count > 0)
        {
            GameObject temp = _ammoUnits[_ammoUnits.Count - 1];
            _ammoUnits.Remove(_ammoUnits[_ammoUnits.Count - 1]);
            Destroy(temp);
        }        
    }

    public void Refill()
    {
        StartCoroutine(RefillCoroutine(0.05f));
    }

    private IEnumerator RefillCoroutine(float delayBetweenUnits)
    {
        while(_ammoUnits.Count < _maxAmmo)
        {
            GameObject ammoUnit = Instantiate(_ammoPrefab, _gridGameObject.transform);
            _ammoUnits.Add(ammoUnit);
            yield return new WaitForSecondsRealtime(delayBetweenUnits);
        }
    }

}
