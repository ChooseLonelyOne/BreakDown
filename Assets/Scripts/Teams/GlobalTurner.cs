using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalTurner : MonoBehaviour
{
    public static Action SwitchTurnAction;

    [SerializeField] private List<TeamManager> _teams;
    private int _chosenTeam;

    public List<TeamManager> Teams => _teams;

    void Start()
    {
        for (int i = 0; i < _teams.Count; i++)
        {
            _teams[i].PlayerListTurnIsEnd += ActivateSwitchTurn;
        }
        StartCoroutine(StartGame());
    }

    private void ActivateSwitchTurn()
    {
        //print(_chosenTeam + " chosen ACTIVATESWITCHTURN 111");
        print("Turn of " + _teams[_chosenTeam].name + " is End");
        if (_chosenTeam < _teams.Count - 1)
        {
            _chosenTeam++;
            //print(_chosenTeam + " chosen ACTIVATESWITCHTURN 222");
            StartCoroutine(SwitchTurn());
            return;
        }
        _chosenTeam = 0;
        //print(_chosenTeam + " chosen ACTIVATESWITCHTURN 333");
        StartCoroutine(SwitchTurn());
    }
    private IEnumerator SwitchTurn()
    {
        //print(_chosenTeam + " chosen SWITCHTURN");
        yield return new WaitForSeconds(.5f);
        print("Next turn is " + _teams[_chosenTeam].name);
        yield return new WaitForSeconds(.5f);
        _teams[_chosenTeam].ActivateTeam();
    }

    private IEnumerator StartGame()
    {
        yield return new WaitForSeconds(1f);
        print("Now i choose who would turn first");
        yield return new WaitForSeconds(1f);
        _chosenTeam = UnityEngine.Random.Range(0, _teams.Count);
        //print(_chosenTeam + " chosen random");
        print("I chosen " + _teams[_chosenTeam].name);
        _teams[_chosenTeam].ActivateTeam();
    }


    private void OnDestroy()
    {
        for (int i = 0; i < _teams.Count; i++)
        {
            _teams[i].PlayerListTurnIsEnd -= ActivateSwitchTurn;
        }
    }
}
