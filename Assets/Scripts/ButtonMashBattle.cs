using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonMashBattle
{

    private ButtonMasher _player1, _player2;

    private float _currentBattleStatus = 0;

    private bool _gameIsDone = false;
    private string _gamewinner;

    // Start is called before the first frame update
    public ButtonMashBattle()
    {
        _player1 = new ButtonMasher(KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D);
        _player2 = new ButtonMasher(KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow);
    }

    // Update is called once per frame
    public void Update()
    {
        if (!_gameIsDone)
        {
            _player1.Update();
            _player2.Update();

            CheckKey(_player1, 1);
            CheckKey(_player2, -1);

            if (_currentBattleStatus >= 10) GameOver("player1");
            if (_currentBattleStatus <= -10) GameOver("player2");
        }

    }

    private void GameOver(string winner)
    {
        _gameIsDone = true;
        _gamewinner = winner;
    }

    private void CheckKey(ButtonMasher player, int playerValue)
    {
        if (Input.GetKeyDown(player.GetCurrentKey()))
        {
            _currentBattleStatus += playerValue;
        }
    }

    public bool IsGameDone()
    {
        return _gameIsDone;
    }

    public string GetGameWinner()
    {
        return _gamewinner;
    }
}
