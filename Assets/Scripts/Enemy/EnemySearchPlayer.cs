using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySearchPlayer : MonoBehaviour
{
    [SerializeField] private List<Transform> _playerList;
    [SerializeField] private List<Vector2> _distanceToPlayers;
    [SerializeField] private List<float> _ratingDistanceToPlayers;
    [SerializeField] private Transform _crash;
    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _distanceToCrash;
    [SerializeField] private float _maxDistanceToTargetX = 1.5f;
    [SerializeField] private float _minDistanceToTargetX = 0f;
    [SerializeField] private float _ratingDistanceToCrash;
    [SerializeField] private bool _check;
    private LayerMask _myLayer;
    [SerializeField] private LayerMask _platformLayer;

    private GameObject _platform;
    public bool Checking => _check;
    private Character _character;
    private GlobalTurnerrr _turner;
    private TeamManagerrr _teamManager;
    private Gun _gun;

    private void Awake()
    {
        _myLayer = gameObject.layer;
        _gun = transform.GetChild(0).GetComponent<Gun>();
        _turner = FindObjectOfType<GlobalTurnerrr>();
        _teamManager = transform.parent.GetComponent<TeamManagerrr>();
        _character = GetComponent<Character>();
        GetPlayerList();
    }

    public void StartAnalys()
    {
        GetPlayerList();
        GetDistanceToPlayers();
        RaitingPlayers();
        GetDistanceToCrash();
        RatingCrash();
        _target = ChooseBestWay();
        MoveToTarget();
    }

    private void GetPlayerList()
    {
        _playerList = new List<Transform>();
        foreach (var team in _turner.Teams)
        {
            if (team.gameObject.layer != transform.gameObject.layer)
            {
                //print(team.PlayerTeam.Count);
                for (int i = 0; i < team.Teammates.Count; i++)
                {
                    _playerList.Add(team.Teammates[i].transform);
                }
            }
        }
    }

    private bool CanGetDamage()
    {
        GetPlayerList();
        foreach (var player in _playerList)
        {
            Vector2 direction = (transform.position - player.position).normalized;
            Vector2 pos = new Vector2(player.position.x + player.GetComponent<SpriteRenderer>().size.x * direction.x, player.position.y);
            RaycastHit2D hit = Physics2D.Raycast(pos, direction, 7f, 1 << _myLayer.value);
            //print(direction);
            if (hit)
            {
                return true;
                /*print(hit.transform.gameObject.layer + " " + _myLayer.value);
                if (hit.transform.gameObject.layer == _myLayer)
                {
                }*/
            }
        }
        return false;
    }
    private void GetDistanceToPlayers()
    {
        _distanceToPlayers = new List<Vector2>();
        foreach (var player in _playerList)
        {
            _distanceToPlayers.Add(transform.position - player.position);
        }
    }

    private void RaitingPlayers()
    {
        _ratingDistanceToPlayers = new List<float>();
        foreach (var distanceToPlayer in _distanceToPlayers)
        {
            float dist = (Mathf.Abs(distanceToPlayer.x) / _character.Speed * _character.Endurance) + (Mathf.Abs(distanceToPlayer.y));
            _ratingDistanceToPlayers.Add(dist);
        }
    }

    private void GetDistanceToCrash()
    {
        if (_crash != null)
        {
            _distanceToCrash = transform.position - _crash.position;
        }
    }

    private void RatingCrash()
    {
        if (_crash != null)
        {
            _ratingDistanceToCrash = (Mathf.Abs(_distanceToCrash.x) / _character.Speed * _character.Endurance) + (Mathf.Abs(_distanceToCrash.y));
        }
    }

    private Transform ChooseBestWay()
    {
        int best = 0;
        float nearest = _ratingDistanceToPlayers[0];

        for (int i = 0; i < _ratingDistanceToPlayers.Count; i++)
        {
            if (_ratingDistanceToPlayers[i] < nearest)
            {
                nearest = _ratingDistanceToPlayers[i];
                best = i;
            }
        }

        if (_crash != null)
        {
            if (_ratingDistanceToPlayers[best] < _ratingDistanceToCrash)
            {
                return _playerList[best].transform;
            }
            return _crash.transform;
        }
        return _playerList[best].transform;
    }

    private void MoveToTarget()
    {
        StartCoroutine(StartMove());
    }

    private void FixedUpdate()
    {
        if (_character.Active)
        {
            _character.FlipRight();
            _character.CheckingGround();
            _character.SetEndurance();
        }
    }

    public IEnumerator StartMove()
    {
        Vector2 spriteRenderer = GetComponent<SpriteRenderer>().size;
        _character.MoveVectorX = Math.Sign(_target.position.x - transform.position.x);
        while (_character.Active || _character.Endurance > 0)
        {
            Vector2 distanceToTarget = new Vector2(transform.position.x - _target.position.x, _target.position.y - transform.position.y);

            if (distanceToTarget.y < -0.2f)
            {
                if (_platform != null)
                {
                    print("враг снизу");
                    if (_platform.transform.position.x < transform.position.x)
                    {
                        _character.MoveVectorX = 1f;
                    }
                    else
                    {
                        _character.MoveVectorX = -1f;
                    }
                    _character.MovePlayer();
                }
            }
            else if(distanceToTarget.y > 0.2f)
            {
                print("враг сверху");
                Vector2 pos = new Vector2(transform.position.x, transform.position.y + spriteRenderer.y/2);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.up, 3f, _platformLayer);
                if (hit)
                {
                    _character.Jump();
                    yield return new WaitForSeconds(1f);
                }
                else
                {
                    _character.MoveVectorX = Math.Sign(_target.position.x - transform.position.x);
                    _character.MovePlayer();
                }
            }
            else if (_character.CheckGround && distanceToTarget.y < 0.2f && distanceToTarget.y > -0.2f)
            {
                if (Mathf.Abs(distanceToTarget.x) < _maxDistanceToTargetX)
                {
                    if (Mathf.Abs(distanceToTarget.x) > _minDistanceToTargetX)
                    {
                        _character.MoveVectorX = Math.Sign(_target.position.x - transform.position.x);
                        Vector2 direction = new Vector2(_character.FlipVector.x, 0).normalized;
                        RaycastHit2D hit = Physics2D.Raycast(_gun.FirePoint.position, direction, _maxDistanceToTargetX);

                        if (hit)
                        {
                            LayerMask layer = hit.transform.gameObject.layer;
                            if (layer != _myLayer)
                            {
                                yield return StartCoroutine(Shoot());
                                break;
                            }
                            _character.MovePlayer();
                        }
                    }
                    else
                    {
                        print("else 1");
                        _character.MoveVectorX = -Math.Sign(_target.position.x - transform.position.x);
                        _character.MovePlayer();
                    }
                }
                else
                {
                    print("else 2 ");
                    _character.MoveVectorX = Math.Sign(_target.position.x - transform.position.x);
                    _character.MovePlayer();
                }
            }
            else if (_character.CheckGround)
            {
                _character.MoveVectorX = Math.Sign(_target.position.x - transform.position.x);
                print("we move");
                _character.MovePlayer();
            }
            yield return new WaitForFixedUpdate();
        }

        if (CanGetDamage())
        {
            print("can get damage");
        }
        yield return new WaitForSeconds(1f);
        if (_character.Endurance > 0 && _character.Active)
        {
            _character.StopTurn();
            print("called stop turn from coroutin");
        }
    }

    private IEnumerator Shoot()
    {
        print("Shoot");
        yield return StartCoroutine(_gun.Shoot());
        yield return new WaitForSeconds(.2f);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        LayerMask layer = col.gameObject.layer;

        if ((1<<layer & _character.PlatformLayerMask) != 0)
        {
            _platform = col.gameObject;
            //print("collision enter");
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        LayerMask layer = col.gameObject.layer;

        if ((1<<layer & _character.PlatformLayerMask) == 0)
        {
            _platform = null;
            //print("collision exit");
        }
    }
}

