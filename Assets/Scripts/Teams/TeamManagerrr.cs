using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TeamManagerrr : MonoBehaviour
{
    public static Action<Transform> SelectedPlayer;
    public static Action UnSelectedPlayer;

    [SerializeField] private List<Character> _teammates;
    [SerializeField] private float _timeForTurn;
    [SerializeField] private Text _timeForTurnText;
    [SerializeField] private Button _skipButton;

    public List<Character> Teammates => _teammates;

    private float _timeForTurnMax;
    private int _currentTeammate = 0;
    private GlobalTurnerrr _turner;

    private void Awake()
    {
        _timeForTurnMax = _timeForTurn;
        _turner = FindObjectOfType<GlobalTurnerrr>();
        UpdateListTeammates();
    }

    private void UpdateListTeammates()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _teammates.Add(transform.GetChild(i).GetComponent<Character>());
        }
    }

    private void Start()
    {
        UnSelectedPlayer?.Invoke();
    }

    private void Update()
    {
        if (_teammates.Count > 0)
        {
            if (_teammates[_currentTeammate].Active)
            {
                _timeForTurnText.text = string.Format("{0:F1}", _timeForTurn);
            }
        }
    }

    public void DesactivateTeam()
    {
        foreach (var teammate in _teammates)
        {
            EnemySearchPlayer search = teammate.GetComponent<EnemySearchPlayer>();
            if (search != null)
            {
                print("desactivate coroutine");
                StopCoroutine(search.StartMove());
            }
            teammate.Active = false;
        }
    }

    public void ActivateTeam()
    {
        if (_teammates.Count > 0)
        {
            _skipButton.onClick.AddListener(SkipTurn);
            _currentTeammate = 0;
            _teammates[_currentTeammate].StartTurn();
            SelectedPlayer?.Invoke(_teammates[_currentTeammate].transform);
            StartCoroutine(StartTimer());
        }
        else
        {
            print("NOT CHARACTERS IN THE TEAM");
        }
    }

    public IEnumerator NextTeammate()
    {
        _skipButton.onClick.RemoveListener(SkipTurn);
        UnSelectedPlayer?.Invoke();
        yield return new WaitForSeconds(0.5f);
        if (_currentTeammate + 1 < _teammates.Count)
        {
            _skipButton.onClick.AddListener(SkipTurn);
            _currentTeammate++;
            _teammates[_currentTeammate].StartTurn();
            SelectedPlayer?.Invoke(_teammates[_currentTeammate].transform);
            StartCoroutine(StartTimer());
        }
        else if(_currentTeammate >= _teammates.Count - 1)
        {
            SwitchTeam();
            print("switch team");
        }
    }

    private IEnumerator StartTimer()
    {
        _timeForTurn = _timeForTurnMax;
        while (_timeForTurn > 0)
        {
            yield return null;
            if (!_teammates[_currentTeammate].Active)
            {
                _timeForTurn = 0f;
                break;
            }
            _timeForTurn -= Time.deltaTime;
        }
        if (_teammates[_currentTeammate].Active)
        {
            _teammates[_currentTeammate].StopTurn();
        }
    }

    public void SwitchTeam()
    {
        _skipButton.onClick.RemoveListener(SkipTurn);
        UnSelectedPlayer?.Invoke();
        StartCoroutine(_turner.SwitchTeam());
    }

    public void DeadTeammate(Character teammate)
    {
        Destroy(teammate.gameObject);
        _teammates.Remove(teammate);
        print("Dead " + teammate.name);
        if (_teammates.Count < 1)
        {
            _turner.TeamEliminated(this);
            return;
        }
        _currentTeammate = 0;
    }

    public void SkipTurn()
    {
        _teammates[_currentTeammate].StopTurn();
    }
}
