using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTurnerrr : MonoBehaviour
{
    [SerializeField] private List<TeamManagerrr> _teams;
    private int _currentTeam = 0;

    public List<TeamManagerrr> Teams => _teams;

    private void Start()
    {
        StartCoroutine(ChooseRandomTeam());
    }

    public IEnumerator ChooseRandomTeam()
    {
        print("Now I choose who would turn first...");
        _currentTeam = Random.Range(0, _teams.Count);
        yield return new WaitForSeconds(0.5f);
        print("I chosen " + _teams[_currentTeam].name);
        yield return new WaitForSeconds(0.5f);
        print("Start Fight");
        ActivateTeam();
    }

    public void ActivateTeam()
    {
        _teams[_currentTeam].ActivateTeam();
    }

    public IEnumerator SwitchTeam()
    {
        if (_teams.Count > 1)
        {
            print("Turn of " + _teams[_currentTeam].name + " is End");
            yield return new WaitForSeconds(0.5f);
            _currentTeam++;
            if (_currentTeam >= _teams.Count)
            {
                _currentTeam = 0;
            }
            print("Next turn is " + _teams[_currentTeam].name);
            yield return new WaitForSeconds(0.5f);
            ActivateTeam();
        }
    }

    public void TeamEliminated(TeamManagerrr loseTeam)
    {
        print(loseTeam.transform.name + " is Eliminated");
        _teams.Remove(loseTeam);
        Destroy(loseTeam.gameObject);
        //print(_teams.Count);
        if (_teams.Count < 2)
        {
            foreach (var team in _teams)
            {
                if (team != loseTeam)
                {
                    team.DesactivateTeam();
                    print(team.transform.name + " Win");
                    //Destroy(team);
                }
            }
        }
    }
}
