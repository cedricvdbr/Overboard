using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class TurnManager : MonoBehaviour
{
    public PlayerControl player1;
    public PlayerControl player2;

    private PlayerControl currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        currentPlayer = player1;

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
        
        if (currentPlayer == player1)
        {
            currentPlayer = player2;
        }
        else
        {
            currentPlayer = player1;
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
