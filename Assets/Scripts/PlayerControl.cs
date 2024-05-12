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

    [SerializeField]
    private LayerMask _wallLayer;

    [SerializeField]
    private GameObject _cannonPrefab;

    [SerializeField]
    private LayerMask _cannonLayer;

    [SerializeField]
    private LayerMask _barrelLayer;

    void Start()
    {
        _gridGenerator = FindFirstObjectByType<GridGenerator>();
        _targetPosition = transform.position;
        _isMoving = false;
        _tileLayer = LayerMask.GetMask("Tile");
        _wallLayer = LayerMask.GetMask("Wall");
        _cannonLayer = LayerMask.GetMask("Cannon");
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
                HandleCannonPlacementP1();
                HandleCannonPlacementP2();
                if (Input.GetKeyDown(KeyCode.V))
                {
                    CannonShooting();
                }
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

    private void HandleCannonPlacementP1()
    {
        if (IsPlayer1BeforeEdge())
        {
            if (Input.GetKeyDown(KeyCode.C) && _turnManager.CannonAmountP1 > 0)
            {
                if (!CheckIfBarrelInFront(transform.position, transform.forward))
                {
                    Vector3 cannonPosition = transform.position + transform.forward;
                    GameObject cannonP1 = Instantiate(_cannonPrefab, cannonPosition, transform.rotation);
                    cannonP1.GetComponent<CannonController>().PlaceCannon();
                    cannonP1.name = "cannonP1";
                    _turnManager.CannonAmountP1--;
                    _turnManager.EndTurn();
                }
            }
        }
    }

    private void HandleCannonPlacementP2()
    {
        if (IsPlayer2BeforeEdge())
        {
            if (Input.GetKeyDown(KeyCode.C) && _turnManager.CannonAmountP2 > 0)
            {
                if (!CheckIfBarrelInFront(transform.position, transform.forward))
                {
                    Vector3 cannonPosition = transform.position + transform.forward;
                    GameObject cannonP2 = Instantiate(_cannonPrefab, cannonPosition, transform.rotation);
                    cannonP2.GetComponent<CannonController>().PlaceCannon();
                    cannonP2.name = "cannonP2";
                    _turnManager.CannonAmountP2--;
                    _turnManager.EndTurn();
                }
            }
        }
    }

    private bool CheckIfBarrelInFront(Vector3 origin, Vector3 direction)
    {
        float maxDistance = 1.0f;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, maxDistance, _barrelLayer))
        {
            Debug.Log("Barrel is in front of the player");

            return true;
        }

        Debug.Log("Barrel is not in front of the player");

        return false;
    }

    private void CannonShooting()
    {
        GameObject cannon = GameObject.Find("cannonP" + PlayerNumber);

        if (cannon != null && cannon.GetComponent<CannonController>() != null && cannon.GetComponent<CannonController>().IsCannonPlaced())
        {
            Vector3 piratePosition = transform.position;
            Vector3 cannonPosition = cannon.GetComponent<CannonController>().GetCannonPosition();

            Vector3 tileBehindCannon = cannonPosition - cannon.transform.forward;

            if (piratePosition == tileBehindCannon)
            {
                cannon.GetComponent<CannonController>().Shoot();
                _turnManager.EndTurn();
            }
            else
            {
                Debug.Log("Pirate is not behind the cannon");
            }
        }
        else
        {
            Debug.Log("Cannon is not placed down");
        }
    }

    private bool IsPlayer1BeforeEdge()
    {
        return name == "pirate1" && transform.position.x == -5
            || name == "pirate2" && transform.position.x == -5
            || name == "pirate3" && transform.position.x == -5;
    }

    private bool IsPlayer2BeforeEdge()
    {
        return name == "pirate4" && transform.position.x == 4
            || name == "pirate5" && transform.position.x == 4
            || name == "pirate6" && transform.position.x == 4;
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

                if (!CheckWallCollision(clickedPosition))
                    return;

                if (!CheckCannonCollision(clickedPosition))
                    return;

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

    private bool CheckCannonCollision(Vector3 targetPosition)
    {
        Vector3 movementDirection = targetPosition - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, movementDirection, out hit, movementDirection.magnitude, _cannonLayer))
        {
            // Cannon detected, prevent movement
            return false;
        }

        // No wall detected, movement is allowed
        return true;
    }

    private bool CheckWallCollision(Vector3 targetPosition)
    {
        Vector3 movementDirection = targetPosition - transform.position;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, movementDirection, out hit, movementDirection.magnitude, _wallLayer))
        {
            // Wall detected, prevent movement
            return false;
        }

        // No wall detected, movement is allowed
        return true;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }
}
