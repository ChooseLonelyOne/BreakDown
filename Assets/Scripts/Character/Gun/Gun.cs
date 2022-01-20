using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private LayerMask _barrel;
    [SerializeField] private GunDatabase _gunData;
    [SerializeField] private LineRenderer _lineRenderer;

    private List<LayerMask> _enemysLayers;
    private GlobalTurner _turner;
    private Transform _firePoint;
    private Character _character;

    public float RateOfFire => _gunData.RateOfFire;
    public float Distance => _gunData.Distance;
    public float Spread => _gunData.Spread;
    public int CountOfBullets => _gunData.CountOfBullets;
    public int Damage => _gunData.Damage;
    public Bullet Bullet => _gunData.Bullet;

    public Transform FirePoint => _firePoint;

    private void Start()
    {
        _enemysLayers = new List<LayerMask>();
        _character = transform.parent.GetComponent<Character>();
        _turner = FindObjectOfType<GlobalTurner>();
        _firePoint = transform.GetChild(0);

        foreach (TeamManager team in _turner.Teams)
        {
            if (team.gameObject.layer != gameObject.layer)
            {
                _enemysLayers.Add(team.gameObject.layer);
            }
        }
    }

    public IEnumerator Shoot()
    {
        _character.Shooted = true;
        for (int i = 1; i <= CountOfBullets; i++)
        {
            for (int j = 0; j < _enemysLayers.Count; j++)
            {
                float spread = Random.Range(-Spread, Spread) / 10;
                //print(spread);
                Vector2 direction = new Vector2(_character.FlipVector.x, 0).normalized;
                RaycastHit2D hitInfo = Physics2D.Raycast(_firePoint.position, direction, Distance);

                if (hitInfo)
                {
                    LayerMask layer = hitInfo.transform.gameObject.layer;

                    if (layer == _enemysLayers[j])
                    {
                        HealthCharacter enemy = hitInfo.transform.GetComponent<HealthCharacter>();
                        enemy.GetDamage(Damage);
                    }
                    else if (layer == gameObject.layer)
                    {
                        print("dont shoot in the friendly");
                    }
                    else if (1<<layer == _barrel)
                    {
                        hitInfo.transform.GetComponent<Barrel>().GetDamage(Damage);
                    }
                    Instantiate(Bullet, _firePoint.position, Quaternion.identity);

                    _lineRenderer.SetPosition(0, _firePoint.position);
                    _lineRenderer.SetPosition(1, new Vector2(hitInfo.point.x, hitInfo.point.y + spread));
                }
                else
                {
                    //print("miss " + i);
                    _lineRenderer.SetPosition(0, _firePoint.position);
                    _lineRenderer.SetPosition(1, new Vector2(_firePoint.position.x + (Distance * direction.x), _firePoint.position.y + spread));
                }

                _lineRenderer.enabled = true;
                yield return new WaitForSeconds(.03f);
                _lineRenderer.enabled = false;
            }
            yield return new WaitForSeconds(RateOfFire);
        }
    }
}
