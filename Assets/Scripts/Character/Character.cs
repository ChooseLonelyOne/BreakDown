using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterDatabase _characterData;

    [SerializeField] private CircleCollider2D _groundCheckCollider;
    [SerializeField] private SpriteRenderer _turnLabel;

    private float _moveVectorX = -1;
    private float _endurance;
    private bool _active;
    private bool _shooted;
    private bool _checkGround;
    private Rigidbody2D _rigidbody2D;
    private TeamManagerrr _teamManager;
    private Vector2 _checkMoves = new Vector2(0, 0);
    private Vector2 _flipVector = new Vector2(1, 1);

    public float Speed => _characterData.Speed;
    public float JumpHeight => _characterData.JumpHeight;
    public float MaxEndurance => _characterData.Endurance;
    public LayerMask PlatformLayerMask => _characterData.Platform;

    public float MoveVectorX { get => _moveVectorX; set => _moveVectorX = value; }
    public float Endurance { get => _endurance; set => _endurance = value; }
    public bool CheckGround { get => _checkGround; set => _checkGround = value; }
    public bool Shooted { get => _shooted; set => _shooted = value; }
    public bool Active { get => _active; set => _active = value; }
    public Vector2 FlipVector => _flipVector;

    private void Awake()
    {
        _endurance = MaxEndurance;
        _flipVector = new Vector2(transform.localScale.x, 1);
        _teamManager = transform.parent.GetComponent<TeamManagerrr>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void FlipRight()
    {
        if (_moveVectorX > 0)
        {
            _flipVector = new Vector2(1, 1);
            transform.localScale = _flipVector;
        }
        if (_moveVectorX < 0)
        {
            _flipVector = new Vector2(-1, 1);
            transform.localScale = _flipVector;
        }
    }

    public void Jump()
    {
        if (_checkGround)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, JumpHeight);
        }
    }

    public void CheckingGround()
    {
        _checkGround = Physics2D.OverlapCircle(_groundCheckCollider.transform.position, _groundCheckCollider.radius, PlatformLayerMask) && _rigidbody2D.velocity.y == 0;
    }

    public void MovePlayer()
    {
        _rigidbody2D.velocity = new Vector2(_moveVectorX * Speed, _rigidbody2D.velocity.y);
    }

    public void SetEndurance()
    {
        if (_rigidbody2D.velocity != _checkMoves)
        {
            _endurance -= Time.deltaTime;
        }
        if (_endurance <= 0)
        {
            StopTurn();
        }
    }

    public void StopTurn()
    {
        if (_teamManager != null)
        {
            print(transform.name + " Stop turn");
            _active = false;
            _endurance = 0f;
            _turnLabel.enabled = false;
            StartCoroutine(_teamManager.NextTeammate());
        }
        //_teamManager.NextTeammate();
        /*if (GetComponent<EnemySearchPlayer>())
        {
            EnemySearchPlayer search = GetComponent<EnemySearchPlayer>();
            StopCoroutine(search.StartMove());
            print("stop coroutine from stopturn");
        }*/
    }
    public void StartTurn()
    {
        if (_teamManager != null)
        {
            print(transform.name + " Start turn");
            _shooted = false;
            _active = true;
            _endurance = MaxEndurance;
            _turnLabel.enabled = true;
            if (GetComponent<EnemySearchPlayer>())
            {
                GetComponent<EnemySearchPlayer>().StartAnalys();
            }
        }
    }

    public void Dead()
    {
        _teamManager.DeadTeammate(this);
    }
}
