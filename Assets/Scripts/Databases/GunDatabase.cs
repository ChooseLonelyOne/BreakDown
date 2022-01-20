using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun/GunDatabase")]
public class GunDatabase : ScriptableObject
{
    [SerializeField] private Bullet _bullet;
    [SerializeField] private int _damage;
    [Range(0, 20)] [SerializeField] private float _distance = 10f;
    [Range(0, 30)] [SerializeField] private int _countOfBullets;
    [Range(0, 10)] [SerializeField] private float _spread;
    [Range(0, 1)] [SerializeField] private float _rateOfFire;

    public int CountOfBullets => _countOfBullets;
    public float RateOfFire => _rateOfFire;
    public float Distance => _distance;
    public float Spread => _spread;
    public int Damage => _damage;
    public Bullet Bullet => _bullet;
}
