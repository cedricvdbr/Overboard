using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TurnManager : MonoBehaviour
{
    public PlayerControl player1Pawn1, player1Pawn2, player1Pawn3;
    public PlayerControl player2Pawn1, player2Pawn2, player2Pawn3;

    private PlayerControl currentPlayer;

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
        Debug.Log("Current player: " + currentPlayer.name);
    }

    void StartPlayerTurn()
    {
        currentPlayer.StartPlayerTurn();
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
