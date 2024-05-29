using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ButtonMashBattle
{

    private ButtonMasher _player1, _player2;

    public float _currentBattleStatus = 0;

    private bool _gameIsDone = false;
    private int _gamewinner;

    protected float _keyFrameTimer;
    protected float _secondsBeforeRenew = 8;
    public int newKeyIndex;

    private AudioSource _swordSwingSound;

    // Start is called before the first frame update
    public ButtonMashBattle()
    {
        _player1 = new ButtonMasher(KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, this);
        _player2 = new ButtonMasher(KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L, this);
        _swordSwingSound = GameObject.Find("SwordSwing").GetComponent<AudioSource>();
        _swordSwingSound.Play();
    }

    // Update is called once per frame
    public void Update()
    {
        _keyFrameTimer += Time.deltaTime;

        if (_keyFrameTimer >= _secondsBeforeRenew)
        {
            _swordSwingSound.Play();
            newKeyIndex = Random.Range(0, 4);
            _keyFrameTimer = 0;
            _secondsBeforeRenew = Random.Range(2, 10);
        }

        if (!_gameIsDone)
        {
            _player1.NewUpdate();
            _player2.NewUpdate();

            CheckKey(_player1, 1);
            CheckKey(_player2, -1);

            //Debug.Log(_currentBattleStatus);
            if (_currentBattleStatus >= 20) GameOver(1);
            if (_currentBattleStatus <= -20) GameOver(2);
        }

    }

    private void GameOver(int winnerNr)
    {
        _gameIsDone = true;
        _gamewinner = winnerNr;
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

    public int GetGameWinner()
    {
        return _gamewinner;
    }

    public KeyCode GetCurrentKey1()
    {
        return _player1.GetCurrentKey();
    }

    public KeyCode GetCurrentKey2()
    {
        return _player2.GetCurrentKey();
    }
}
