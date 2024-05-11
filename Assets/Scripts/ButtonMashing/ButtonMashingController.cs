using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMashingController : MonoBehaviour
{
    private ButtonMashBattle _buttonmashBattle;
    public Canvas _canvas;
    [SerializeField] private RectTransform _mashUI;

    private bool _startGame = false;

    // Start is called before the first frame update
    void Start()
    {
        _mashUI = GameObject.Find("Middle").GetComponent<RectTransform>();
        _canvas = GameObject.Find("ButtonMashUI").GetComponent<Canvas>();
        _canvas.enabled = true;
        _buttonmashBattle = new ButtonMashBattle();
    }

    // Update is called once per frame
    void Update()
    {
        _mashUI.localPosition = new Vector3(_buttonmashBattle._currentBattleStatus * 27.5f, _mashUI.localPosition.y, _mashUI.localPosition.z);
        //if (Input.GetMouseButtonDown(1)) StartMashing();

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

    public bool ButtonmashingIsDone()
    {
        return _buttonmashBattle.IsGameDone();
    }

    public int GetWinner()
    {
        return _buttonmashBattle.GetGameWinner();

    }
}
