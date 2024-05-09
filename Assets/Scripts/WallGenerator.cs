using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    private MainShipselectController _mainShipselectController;
    [SerializeField]
    private GameObject _wall, _layout1, _layout2, _layout3;
    [SerializeField]
    private int _player1Ship, _player2Ship;
    private void Start()
    {
        _mainShipselectController = FindAnyObjectByType<MainShipselectController>();
        _player1Ship = _mainShipselectController.GetPlayerShip(1);
        _player2Ship = _mainShipselectController.GetPlayerShip(2);
        GenerateWallsP1();
        GenerateWallsP2();
    }

    private void GenerateWallsP1()
    {
        switch (_player1Ship)
        {
            case 0:
                Instantiate(_layout1, new Vector3(-0.5f,0,0), Quaternion.identity);
                break;
            case 1:
                Instantiate(_layout2, new Vector3(-0.5f, 0, 0), Quaternion.identity);
                break;
            case 2:
                Instantiate(_layout3, new Vector3(-0.5f, 0, 0), Quaternion.identity);
                break;
        }
    }
    private void GenerateWallsP2()
    {
        switch (_player2Ship)
        {
            case 0:
                Instantiate(_layout1, new Vector3(-0.5f,0,0), Quaternion.AngleAxis(180f,Vector3.up));
                break;
            case 1:
                Instantiate(_layout2, new Vector3(-0.5f, 0, 0), Quaternion.AngleAxis(180f, Vector3.up));
                break;
            case 2:
                Instantiate(_layout3, new Vector3(-0.5f, 0, 0), Quaternion.AngleAxis(180f, Vector3.up));
                break;
        }
    }
}
