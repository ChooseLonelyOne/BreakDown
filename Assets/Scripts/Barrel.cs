using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IDamage
{
    [SerializeField] private float _health;
    [SerializeField] private int _damage;
    [SerializeField] private bool _redBarrel;
    private GameObject _explose;

    public int Damage => _damage;

    private void Start()
    {
        if (_redBarrel)
        {
            _explose = transform.GetChild(0).gameObject;
        }
    }

    public void GetDamage(int damage)
    {
        _health =_health - damage;
        if (_health <= 0)
        {
            if (_redBarrel)
            {
                StartCoroutine(Explosion());
                return;
            }
            Destroy(gameObject);
        }
    }

    private IEnumerator Explosion()
    {
        _explose.SetActive(true);
        yield return new WaitForSeconds(.3f);
        Destroy(gameObject);
    }
}
