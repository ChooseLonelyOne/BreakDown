using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCharacter : MonoBehaviour
{
    [SerializeField] private float _health = 50;
    [SerializeField] private SpriteRenderer _healthbar;
    [SerializeField] private LayerMask _canGetDamage;
    private float _maxHealthbarSize;
    private float _maxHealth;
    private Character _character;

    private void Start()
    {
        _character = GetComponent<Character>();
        _maxHealth = _health;
        _maxHealthbarSize = _healthbar.size.x;
    }

    public void GetDamage(int damage)
    {
        _health -= damage;
        float health = _maxHealthbarSize / _maxHealth * _health;
        if (_health <= 0)
        {
            _health = 0;
            _healthbar.size = new Vector2(0, _healthbar.size.y);
            _character.Dead();
            return;
        }
        _healthbar.size = new Vector2(health, _healthbar.size.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (1 << collision.gameObject.layer == _canGetDamage)
        {
            IDamage damage = collision.transform.parent.GetComponent<IDamage>();
            if (damage != null)
            {
                print("trigger " + collision.transform.name);
                GetDamage(damage.Damage);
            }
        }
    }
}
