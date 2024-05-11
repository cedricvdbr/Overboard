using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;


public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5.0f;
    public Vector3 _targetPosition;
    [SerializeField]
    private int _remainingMovementSteps = 4, _bridgeAmount=1;
    private bool _isMoving;
    [SerializeField]
    private bool _isPlayerTurn = false, _isMashing;
    public bool IsKO;
    private TurnManager _turnManager;
    public int PlayerNumber;
    private ButtonMashingController _buttonMashingController;

    public bool IsCarribeanRum = false;
    [SerializeField]
    private bool isBeforeMovement = true, _hasBeenUsed = false;

    private PlayerControl[] _players;
    private PlayerControl _clickedPlayer;

    private GridGenerator _gridGenerator;
    private LayerMask _tileLayer;
    private bool _isRandomAbility = false;

    [SerializeField]
    private GameObject _arrowPrefab, _xPrefab,_buttonMasherPrefab, _buttonMasher;
    private GameObject _arrowInstance, _xInstance;

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

        if (_isMashing)
        {
            if (_buttonMashingController.ButtonmashingIsDone())
            {
                _buttonMashingController._canvas.enabled = false;
                int winner = _buttonMashingController.GetWinner();
                if (winner == PlayerNumber)
                {
                    _clickedPlayer.IsKO = true;
                    _clickedPlayer.PlaceX();
                    _turnManager.EndTurn();
                }
                else
                {
                    _turnManager.EndTurn();
                    IsKO = true;
                    PlaceX();
                }
                _isMashing = false;
                Destroy(_buttonMasher);
            }

        }
        else
        {

            if (_isMoving)
            {
                MovePlayer();
            }
            else if (_isPlayerTurn && !IsKO)
            {
                if (Input.GetKeyDown(KeyCode.A) && isBeforeMovement && _isPlayerTurn && !_hasBeenUsed)
                {
                    IsCarribeanRum = true;
                    _hasBeenUsed = true;
                }
                HandleBridge();
                HandleAbilities();
                HandleInput();
                if(Input.GetKeyDown(KeyCode.S))
                {
                    _remainingMovementSteps = 1000;
                    _bridgeAmount = 50;
                    GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
            else if (_isPlayerTurn && IsKO)
            {
                _turnManager.EndTurn();
                IsKO = false;
            }
            if (_remainingMovementSteps >= 4)
            {
                isBeforeMovement = true;
            }
        }
    }
    public void PlaceX()
    {
        if (_xInstance == null && _xPrefab != null)
        {
            _xInstance = Instantiate(_xPrefab, transform.position + Vector3.up * 3.0f, Quaternion.identity);
            _xInstance.transform.parent = transform;
        }
    }

    private void HandleBridge()
    {
        if (IsPlayer1OnEdge() || IsPlayer2OnEdge())
        {
            if (Input.GetKeyDown(KeyCode.B) && _bridgeAmount > 0)
            {
                _gridGenerator.GenerateBridge(gameObject.transform.position, gameObject.name);
                _bridgeAmount--;
            }
        }
    }

    private bool IsPlayer2OnEdge()
    {
        return name == "pirate4" && transform.position.x == 3
            || name == "pirate5" && transform.position.x == 3
            || name == "pirate6" && transform.position.x == 3;
    }

    private bool IsPlayer1OnEdge()
    {
        return name == "pirate1" && transform.position.x == -4 
            || name == "pirate2" && transform.position.x == -4 
            || name == "pirate3" && transform.position.x == -4;
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
                        PlayerControl playerOnTile = TileHasPlayer(clickedPosition);
                        if (playerOnTile != null) AttackPlayer(playerOnTile);
                        else
                        {
                            _targetPosition = new Vector3(clickedPosition.x, transform.position.y, clickedPosition.z);
                            _isMoving = true;
                            _remainingMovementSteps--;
                        }
                    }
                                
                }
                        
            } 
        }
        
    }

    private PlayerControl TileHasPlayer(Vector3 clickedPosition)
    {
        foreach (PlayerControl controller in _players)
        {
            if (controller._targetPosition == clickedPosition && controller.PlayerNumber != PlayerNumber) return controller;
        }
        return null;
    }

    private void AttackPlayer(PlayerControl playerOnTile)
    {
        _clickedPlayer = playerOnTile;
        _buttonMasher = Instantiate(_buttonMasherPrefab, Vector3.zero, Quaternion.identity);
        _buttonMashingController = _buttonMasher.GetComponent<ButtonMashingController>();
        _buttonMashingController.StartMashing();
        _isMashing = true;

        
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
        if(_arrowInstance == null && _arrowPrefab != null)
        {
            _arrowInstance = Instantiate(_arrowPrefab, transform.position + Vector3.up * 3.0f, Quaternion.identity);
            _arrowInstance.transform.parent = transform;
        }
    }

    public void EndPlayerTurn()
    {
        _remainingMovementSteps = 4;
        _isPlayerTurn = false;
        _hasBeenUsed = false;
        if(_arrowInstance != null)
        {
            Destroy(_arrowInstance);
            _arrowInstance = null;
        } 
        if(_xInstance != null)
        {
            Destroy(_xInstance);
            _xInstance = null;
        }
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

    public void GetPlayers()
    {
        _players = Component.FindObjectsByType<PlayerControl>(FindObjectsSortMode.InstanceID);
    }
}
