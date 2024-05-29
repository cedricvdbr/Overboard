using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public PlayerControl player1Pawn1, player1Pawn2, player1Pawn3;
    public PlayerControl player2Pawn1, player2Pawn2, player2Pawn3;

    public PlayerControl currentPlayer;
    public int CurrentTurn = 0;

    private int _player1TreasuresLeft = 3, _player2TreasuresLeft = 3;
    private bool _isGameOver = false;
    private int _cannonAmountP1 = 1, _cannonAmountP2 = 1;

    public int CannonAmountP1
    {
        get { return _cannonAmountP1; }
        set { _cannonAmountP1 = value; }
    }

    public int CannonAmountP2
    {
        get { return _cannonAmountP2; }
        set { _cannonAmountP2 = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        player1Pawn1 = GameObject.Find("pirate1").GetComponent(typeof(PlayerControl)) as PlayerControl;
        player1Pawn2 = GameObject.Find("pirate2").GetComponent(typeof(PlayerControl)) as PlayerControl;
        player1Pawn3 = GameObject.Find("pirate3").GetComponent(typeof(PlayerControl)) as PlayerControl;
        player2Pawn1 = GameObject.Find("pirate4").GetComponent(typeof(PlayerControl)) as PlayerControl;
        player2Pawn2 = GameObject.Find("pirate5").GetComponent(typeof(PlayerControl)) as PlayerControl;
        player2Pawn3 = GameObject.Find("pirate6").GetComponent(typeof(PlayerControl)) as PlayerControl;
        currentPlayer = player1Pawn1;

        StartPlayerTurn();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current player: " + currentPlayer.name);
    }

    public void ChangePlayer1Treasure(int change)
    {
        _player1TreasuresLeft += change;
        if (_player1TreasuresLeft <= 0) GameOver(1);
    }

    public void ChangePlayer2Treasure(int change)
    {
        _player2TreasuresLeft += change;
        if (_player2TreasuresLeft <= 0) GameOver(2);
    }

    private void GameOver(int winnerNr)
    {
        StopGame();
        ShowWinnerScreen(winnerNr);
    }

    private void StopGame()
    {
        _isGameOver = true;
        EndTurn();
    }

    private void ShowWinnerScreen(int winnerNr)
    {
        if (winnerNr == 1) UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        else if (winnerNr == 2) UnityEngine.SceneManagement.SceneManager.LoadScene(4);
    }

    void StartPlayerTurn()
    {
        if (!_isGameOver)
        {
            CurrentTurn++;
            currentPlayer.StartPlayerTurn();
        }
    }

    public void EndTurn()
    {
        currentPlayer.EndPlayerTurn();
        
        if (currentPlayer == player1Pawn1)
        {
            currentPlayer = player1Pawn2;
        }
        else if (currentPlayer == player1Pawn2)
        {
            currentPlayer = player1Pawn3;
        }
        else if (currentPlayer == player1Pawn3)
        {
            currentPlayer = player2Pawn1;
        }
        else if (currentPlayer == player2Pawn1)
        {
            currentPlayer = player2Pawn2;
        }
        else if (currentPlayer == player2Pawn2)
        {
            currentPlayer = player2Pawn3;
        }
        else
        {
            currentPlayer = player1Pawn1;
        }

        Debug.Log("Current player after turn end: " + currentPlayer.name);

        StartCoroutine(WaitForPlayerMovementToEnd());
    }

    IEnumerator WaitForPlayerMovementToEnd()
    {
        yield return new WaitUntil(() => !currentPlayer.IsMoving());
        StartPlayerTurn();
    }

    public void OnTileClicked(Vector3 targetPosition)
    {
        if (!currentPlayer.IsMoving())
        {
            currentPlayer.SetTargetPosition(targetPosition);
        }
    }
}
