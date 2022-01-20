using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Gun))]
public class GunController : MonoBehaviour
{
    private Character _character;
    private Gun _gun;

    private void Start()
    {
        _character = transform.parent.GetComponent<Character>();
        //print(_character);
        _gun = GetComponent<Gun>();
    }

    private void Update()
    {
        if (_character.Active && !_character.Shooted)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _gun.StartCoroutine(_gun.Shoot());
            }
        }
    }
}
