using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask _platform;
    private Vector2 _force = new Vector2(100, 0);
    private Rigidbody2D _rigidbody2D;
    private List<LayerMask> _enemys;
    private float _moveVector;
    private int _damage;

    /*public void Spawn(float moveVector, List<LayerMask> enemys,int damage)
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _moveVector = moveVector;
        _damage = damage;
        _enemys = enemys;
        _force = new Vector2(100 * _moveVector, 0);
        _rigidbody2D.velocity = _force;
    }*/

    /*private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(_spawnPos.position, new Vector2(_moveVector, 0 ), _distance);
        if (hit)
        {
            LayerMask enemy = hit.transform.gameObject.layer;
            Transform enemyTransform = hit.transform;
            for (int i = 0; i < _enemys.Count; i++)
            {
                print(_enemys[i].value);
                print(enemy.value + " hit");
                if (enemy == _enemys[i])
                {
                    enemyTransform.GetComponent<HealthCharacter>().GetDamage(_damage);
                    Destroy(gameObject);
                    return;
                }
            }
        }
    }*/

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject col = collision.gameObject;

        if (col.layer == _platform)
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < _enemys.Count; i++)
        {
            if (col.layer == _enemys[i])
            {
                col.transform.GetComponent<HealthCharacter>().GetDamage(_damage);
                Destroy(gameObject);
                return;
            }
        }
    }*/
}
