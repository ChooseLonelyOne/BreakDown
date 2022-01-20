using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public class PlayerMoveController : MonoBehaviour
{
    private Character _character;

    private void Start()
    {
        _character = GetComponent<Character>();
    }

    private void Update()
    {
        if (_character.Active)
        {
            _character.MoveVectorX = Input.GetAxisRaw("Horizontal");
            _character.FlipRight();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _character.Jump();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_character.Active)
        {
            _character.MovePlayer();
            _character.SetEndurance();
            _character.CheckingGround();
        }
    }
}
