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
    [SerializeField]
    private int _remainingMovementSteps = 4, _bridgeAmount=1;
    private bool _isMoving;
    [SerializeField]
    private bool _isPlayerTurn = false;
    private TurnManager _turnManager;

    public bool IsCarribeanRum = false;
    [SerializeField]
    private bool isBeforeMovement = true, _hasBeenUsed = false;

    private GridGenerator _gridGenerator;
    private LayerMask _tileLayer;
    private bool _isRandomAbility = false;

    void Start()
    {
        _gridGenerator = FindFirstObjectByType<GridGenerator>();
        _targetPosition = transform.position;
        _isMoving = false;
        _tileLayer = LayerMask.GetMask("Tile");

    }

    void Update()
    {
        _turnManager = FindObjectOfType<TurnManager>();

        if (_isMoving)
        {
            MovePlayer();
        }
        else if (_isPlayerTurn)
        {
            if (Input.GetKeyDown(KeyCode.A) && isBeforeMovement && _isPlayerTurn && !_hasBeenUsed)
            {
                IsCarribeanRum = true;
                _hasBeenUsed=true;
            }
            HandleBridge();
            HandleAbilities();
            HandleInput();
        }
        if (_remainingMovementSteps >= 4)
        {
            isBeforeMovement = true;
        }
    }

    private void HandleBridge()
    {
        if (Input.GetKeyDown(KeyCode.B) && _bridgeAmount >0)
        {
            _gridGenerator.GenerateBridge(gameObject.transform.position, gameObject.name); 
        }
    }

    private void HandleInput()
    {

        if (_remainingMovementSteps > 0 && !_isMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _tileLayer) && !_isMoving)
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

    private void HandleAbilities()
    {
        if (AbilityIsOn(ref IsCarribeanRum))    
        {
            _remainingMovementSteps += 2;
        }
        isBeforeMovement = false;
    }

    private bool AbilityIsOn(ref bool currentAbility)
    {
        if (currentAbility)
        {
            currentAbility = !currentAbility;
            return true;
        }
        return false;
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
        _hasBeenUsed = false;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Barrel"))
        {
            Destroy(other.gameObject);
            _isRandomAbility = true;
            Debug.Log("Barrel destroyed, random ability obtained...");
        }
    }
}
