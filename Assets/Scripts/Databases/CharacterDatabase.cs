using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Character/CharacterDatabase")]
public class CharacterDatabase : ScriptableObject
{
    [Range(0, 10)] [SerializeField] private float _endurance;
    [Range(0, 30)] [SerializeField] private float _speed;
    [Range(0, 100)] [SerializeField] private float _jumpheight;

    [SerializeField] private LayerMask _platform;

    public float Endurance => _endurance;
    public float Speed => _speed;
    public float JumpHeight => _jumpheight;
    public LayerMask Platform => _platform;
}
