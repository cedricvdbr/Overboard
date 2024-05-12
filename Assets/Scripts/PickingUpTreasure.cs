using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PickingUpTreasure : MonoBehaviour
{
    //player needs tag player, same w Treasure. Treasure has rigidbody and is kinematic.
    //Treasure collider is assigned to Player(capsule collider)
    private bool isTouchingPlayer = false, IsFollowingPlayer = false;
    private GameObject _playerGO;
    private PlayerControl _playerPC;
    private GameObject _pickupTilePlayer1, _pickupTilePlayer2;
    private TurnManager _turnManager;


    private void Start()
    {
        _pickupTilePlayer1 = GameObject.Find("player1PickupTile");
        _pickupTilePlayer2 = GameObject.Find("player2PickupTile");
    }

    void Update()
    {
        //_turnManager = GetComponent<TurnManager>();
        GameObject _turnManagerGO = GameObject.Find("TurnManager");
        if (_turnManagerGO != null ) _turnManager = _turnManagerGO.GetComponent<TurnManager>();
        if (isTouchingPlayer)
        {
            Vector3 newPosition = _playerGO.transform.position;
            newPosition.y += 2;
            transform.position = newPosition;
            if (_turnManager != null) CheckForCollectionSpot();
        }
    }

    private void CheckForCollectionSpot()
    {
        Vector3 actualPosition = transform.position;
        actualPosition.y -= 2;
        if (actualPosition == _pickupTilePlayer1.transform.position && _playerPC.PlayerNumber == 1)
        {
            _turnManager.ChangePlayer1Treasure(-1);
            RemoveTreasure();
        }
        if (actualPosition == _pickupTilePlayer2.transform.position && _playerPC.PlayerNumber == 2)
        {
            _turnManager.ChangePlayer2Treasure(-1);
            RemoveTreasure();
        }
    }

    private void RemoveTreasure()
    {
        this.gameObject.SetActive(false);
        this.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isTouchingPlayer)
            {
                _playerGO = other.gameObject;
                _playerPC = _playerGO.GetComponent<PlayerControl>();
                Debug.Log("treasure " + name + " is touching player");

                if ((_playerPC.PlayerNumber == 1 && name == "player1treasure") || (_playerPC.PlayerNumber == 2 && name == "player2treasure")) return;
                if (_playerPC.IsCarryingTreasure) return;

                isTouchingPlayer = true;
                IsFollowingPlayer = true;
                _playerPC.IsCarryingTreasure = true;
            }
        }

    }
}
