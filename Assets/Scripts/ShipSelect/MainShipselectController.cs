using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainShipselectController : MonoBehaviour
{

    [SerializeField]
    PlayerControl _player1, _player2;

    private Scene _mainScene, _shipSelectScene;

    private static int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (count == 0) SwitchScenes();
    }

    private void SwitchScenes()
    {
        SceneManager.LoadScene(1);
        count++;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int GetPlayerShip(int player)
    {
        if (SelectShip.IsDone)
        {
            if (player == 1) return SelectShip.Player1Ship;
            if (player == 2) return SelectShip.Player2Ship;
            return 0;
        }
        return -1;
    }

}
