using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    private Vector3 _targetPosition;
    private int _remainingMovementSteps = 4;
    private bool _isMoving;
    private bool _isPlayerTurn = false;
    private TurnManager _turnManager;
    // Start is called before the first frame update
    void Start()
    {
        _targetPosition = transform.position;
        _isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        _turnManager = FindObjectOfType<TurnManager>();

        if (_isMoving)
        {
            MovePlayer();
        }
        else if (_isPlayerTurn)
        {
            HandleInput();
        }
    }



    private void HandleInput()
    {
        if (_remainingMovementSteps > 0 && !_isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.CompareTag("Tile")&& !_isMoving)
            {
                Vector3 clickedPosition = hit.transform.position;
                Vector3 difference = clickedPosition - transform.position;
                if (Mathf.Abs(difference.x) == 1.0f && Mathf.Approximately(difference.z, 0.0f) ||
                    Mathf.Abs(difference.z) == 1.0f && Mathf.Approximately(difference.x, 0.0f))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _targetPosition = new Vector3(clickedPosition.x, transform.position.y, clickedPosition.z);
                        _isMoving = true;
                        _remainingMovementSteps--;
                    }
                                
                }
                        
            } 
        }
        
    }

    private void MovePlayer()
    {
        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

            if (transform.position == _targetPosition)
            {
                _isMoving = false;

                if (_remainingMovementSteps == 0)
                {
                    _turnManager.EndTurn();
                }
            }
        }
    }

    public void StartPlayerTurn()
    {
        _isPlayerTurn = true;
    }

    public void EndPlayerTurn()
    {
        _remainingMovementSteps = 4;
        _isPlayerTurn = false;
    }

    public void SetTargetPosition(Vector3 target)
    {
        _targetPosition = new Vector3(target.x, transform.position.y, target.z);
        _isMoving = true;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
}
