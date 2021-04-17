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

    [SerializeField]
    private int _maxAmmo = 15;

    private void Start()
    {
        for (int i = 0; i < _maxAmmo; i++)
        {
            GameObject ammoUnit = Instantiate(_ammoPrefab, _gridGameObject.transform);
            _ammoUnits.Add(ammoUnit);
        }
    }

    public void DisplayAmmo(int ammoRemaining)
    {

    }


}
