using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButtonMashing : MonoBehaviour
{
    private ButtonMashBattle _buttonmashBattle;

    private bool _startGame = false;

    // Start is called before the first frame update
    void Start()
    {
        _buttonmashBattle = new ButtonMashBattle();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(1)) StartMashing();

        if (_startGame)
        {
            _buttonmashBattle.Update();

            _startGame = !_buttonmashBattle.IsGameDone();


            //Debug.Log(_buttonmashBattle.IsGameDone());
            Debug.Log("key 1: " + _buttonmashBattle.GetCurrentKey1());
            Debug.Log("key 2: " + _buttonmashBattle.GetCurrentKey2());

            if (_buttonmashBattle.IsGameDone())
            {
                Debug.Log("winner: " + _buttonmashBattle.GetGameWinner());
            }
        }
    }

    public void StartMashing()
    {
        _startGame = true;
    }
}
