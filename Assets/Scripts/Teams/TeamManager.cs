using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TeamManager : MonoBehaviour
{
    public Action PlayerListTurnIsEnd;
    public static Action<Transform> SelectedPlayer;
    public static Action UnSelectedPlayer;

    [SerializeField] private List<Transform> _playerTeam;
    [SerializeField] private float _timeForTurn;
    [SerializeField] private Text _timeForTurnText;
    private int _currentPlayer = 0;
    private Button _skipBtn;

    public int CurrentPlayer => _currentPlayer;
    public List<Transform> PlayerTeam => _playerTeam;

    private void Update()
    {
        if (_playerTeam.Count != 0)
        {
            if (_playerTeam[_currentPlayer].GetComponent<Character>().Active)
            {
                _timeForTurnText.text = string.Format("{0:F1}", _timeForTurn);
            }
        }
    }

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _playerTeam.Add(transform.GetChild(i));
        }
    }

    private void Start()
    {
        _skipBtn = FindObjectOfType<Canvas>().transform.GetChild(2).GetComponent<Button>();
        UnSelectedPlayer?.Invoke();
        GlobalTurner.SwitchTurnAction += ActivateTeam;
    }

    public void ActivateTeam()
    {
        if (_playerTeam.Count != 0)
        {
            _skipBtn.onClick.AddListener(SkipTurn);
            _currentPlayer = 0;
            _playerTeam[_currentPlayer].GetComponent<Character>().StartTurn();
            SelectedPlayer?.Invoke(_playerTeam[_currentPlayer]);
            StartCoroutine(StartTimer());
            return;
        }
        print(transform.name + " Lose!!!");
    }

    public void NextPlayer()
    {
        if (_currentPlayer + 1 <= PlayerTeam.Count - 1)
        {
            StartCoroutine(ActivateNextPlayer());

            _currentPlayer++;
            return;
        }
        UnSelectedPlayer?.Invoke();
        if (!_playerTeam[_currentPlayer].GetComponent<Character>().Active)
        {
            print("Endurance is end, next team");
        }
        else
        {
            print("Time is end, next team");
        }
        _currentPlayer = 0;
        _skipBtn.onClick.RemoveListener(SkipTurn);
        PlayerListTurnIsEnd?.Invoke();
    }
    public void Dead(Transform player)
    {
        Destroy(player.gameObject);
        _playerTeam.Remove(player);
    }

    private void SkipTurn()
    {
        _playerTeam[_currentPlayer].GetComponent<Character>().StopTurn();
    }

    private IEnumerator StartTimer()
    {
        _timeForTurn = 15f;
        while (_timeForTurn > 0)
        {
            yield return null;
            if (!_playerTeam[_currentPlayer].GetComponent<Character>().Active)
            {
                _timeForTurn = 0f;
                break;
            }
            _timeForTurn -= Time.deltaTime;
        }
        if (_playerTeam[_currentPlayer].GetComponent<Character>().Active)
        {
            _playerTeam[_currentPlayer].GetComponent<Character>().StopTurn();
        }
    }

    private IEnumerator ActivateNextPlayer()
    {
        UnSelectedPlayer?.Invoke();
        if (!_playerTeam[_currentPlayer].GetComponent<Character>().Active)
        {
            print("Endurance is end, next player");
        }
        else
        {
            print("Time is end, next player");
        }
        yield return new WaitForSeconds(1f);
        _playerTeam[_currentPlayer].GetComponent<Character>().StartTurn();
        SelectedPlayer?.Invoke(_playerTeam[_currentPlayer]);
        StartCoroutine(StartTimer());
    }

    private void OnDestroy()
    {
        GlobalTurner.SwitchTurnAction -= ActivateTeam;
    }
}
