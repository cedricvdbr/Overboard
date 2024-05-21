using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    //private List<bool> _abilities = new List<bool>();
    //private bool _isCarribeanRum;
    private int[] values = new int[] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 7, 7 };
    private string[] _abilityNames = new string[] { "Harpoon Gun", "Caribbean Rum", "Cursed Compass", "Parrot's Warning", "Broken Cannon Ball", "Curse Of The Flying Dutchman", "Fortify", "Davy Jones Locker" };
    private bool[] _availableAbilities = new bool[] { false, false, false, false, false, false, false, false };
    private bool[] _abilities = new bool[] { false, false, false, false, false, false, false, false };
    private bool _brokenCannon = false, _canGoThroughWall = false;
    private bool _cantBeKOd = false;
    private int _cantBeKOdCounter = 0;
    private int _currentAbilityIndex = -1;
    [SerializeField]
    private bool isBeforeMovement = true;

    public bool IsCarryingTreasure = false;

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

    [SerializeField]
    private LayerMask _playerLayer;

    private bool _playerHasBeenSelected = false;
    private PlayerControl _hitPlayer;
    private GameObject _gridgenGO;
    private GridGenerator _gridgen;
    private List<GameObject> _player1Tiles, _player2Tiles;

    [SerializeField]
    private GameObject _wallPrefab;

    void Start()
    {
        _gridGenerator = FindFirstObjectByType<GridGenerator>();
        _targetPosition = transform.position;
        _isMoving = false;
        _tileLayer = LayerMask.GetMask("Tile");
        _wallLayer = LayerMask.GetMask("Wall");
        _cannonLayer = LayerMask.GetMask("Cannon");
        _playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        _gridgenGO = GameObject.Find("GridGenerator");
        if (_gridgenGO != null )
        {
            _gridgen = _gridgenGO.GetComponent<GridGenerator>();
            _player1Tiles = _gridgen.TilePositionsP1;
            _player2Tiles = _gridgen.TilePositionsP2;
        }
        _turnManager = FindObjectOfType<TurnManager>();

        if (_cantBeKOd && IsKO)
        {
            IsKO = false;
            Destroy(_xInstance);
            _xInstance = null;
        }

        if (_cantBeKOd && _cantBeKOdCounter == 1)
        {
            _cantBeKOd = false;
            _cantBeKOdCounter = 0;
        }

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
            if (_isRandomAbility)
            {
                bool chooseANewOne = true;
                foreach (bool ability in _abilities)
                {
                    if (ability) chooseANewOne = false;
                }

                if (chooseANewOne)
                {
                    //int chosenAbility = UnityEngine.Random.Range(0, _abilities.Length);
                    int randomIndex = UnityEngine.Random.Range(0, values.Length);
                    int chosenAbility = values[randomIndex];
                    _availableAbilities[chosenAbility] = true;
                    _currentAbilityIndex = chosenAbility;
                }

                _isRandomAbility = false;
            }
            if (_isMoving)
            {
                MovePlayer();
            }
            else if (_isPlayerTurn && !IsKO)
            {
                HandleBridge();
                HandleCannonPlacementP1();
                HandleCannonPlacementP2();
                if (Input.GetKeyDown(KeyCode.V))
                {
                    CannonShooting();
                }
                HandleAbilities();
                HandleInput();
                if(Input.GetKeyDown(KeyCode.Equals))
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

    public string GetCurrentAbilityName()
    {
        if (_currentAbilityIndex == -1) return "None";
        return _abilityNames[_currentAbilityIndex];
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
                if (!CheckIfBarrelInFront(transform.position, transform.forward) && !CheckIfPlayerInFront(transform.position, transform.forward))
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
                if (!CheckIfBarrelInFront(transform.position, transform.forward) && !CheckIfPlayerInFront(transform.position, transform.forward))
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

    private bool CheckIfPlayerInFront(Vector3 origin, Vector3 direction)
    {
        float maxDistance = 1.0f;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction, out hit, maxDistance, _playerLayer))
        {
            Debug.Log("Player is in front of the player");

            return true;
        }

        Debug.Log("Player is not in front of the player");

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
                if (_brokenCannon)
                {
                    CannonBallMovement._playerDestroyChance = 0.5f;
                    _brokenCannon = false;
                    Debug.Log("brokencannon has been used");
                }
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
        int result = IsPlayerOnEdge();
        if (result == 1 || result == -1)
        {
            if (Input.GetKeyDown(KeyCode.B) && _bridgeAmount > 0)
            {
                bool isntPlaced = _gridGenerator.GenerateBridge(gameObject.transform.position, gameObject.name, result);
                if (!isntPlaced) _bridgeAmount--;
            }
        }
    }

    private int IsPlayerOnEdge()
    {
        if (name == "pirate1" && transform.position.x == -4
            || name == "pirate2" && transform.position.x == -4
            || name == "pirate3" && transform.position.x == -4)
            return 1;
        if (name == "pirate1" && transform.position.x == 3
            || name == "pirate2" && transform.position.x == 3
            || name == "pirate3" && transform.position.x == 3)
            return -1;

        if (name == "pirate4" && transform.position.x == 3
            || name == "pirate5" && transform.position.x == 3
            || name == "pirate6" && transform.position.x == 3)
            return -1;
        if (name == "pirate4" && transform.position.x == -4
            || name == "pirate5" && transform.position.x == -4
            || name == "pirate6" && transform.position.x == -4)
            return 1;

        return 0;
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

                if (!CheckWallCollision(clickedPosition) && !_canGoThroughWall)
                    return;

                if (!CheckCannonCollision(clickedPosition))
                    return;

                if (Mathf.Abs(difference.x) == 1.0f && Mathf.Approximately(difference.z, 0.0f) ||
                    Mathf.Abs(difference.z) == 1.0f && Mathf.Approximately(difference.x, 0.0f))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        PlayerControl playerOnTile = TileHasEnemy(clickedPosition);
                        if (playerOnTile != null) AttackPlayer(playerOnTile);
                        else if (!TileHasPirate(clickedPosition))
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

    private PlayerControl TileHasEnemy(Vector3 clickedPosition)
    {
        foreach (PlayerControl controller in _players)
        {
            if (controller._targetPosition == clickedPosition && controller.PlayerNumber != PlayerNumber) return controller;
        }
        return null;
    }

    private bool TileHasPirate(Vector3 clickedPosition)
    {
        foreach (PlayerControl controller in _players)
        {
            if (controller._targetPosition == clickedPosition && controller.PlayerNumber == PlayerNumber) return true;
        }
        return false;
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
        if (!isBeforeMovement && !_availableAbilities[6]) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            for (int i = 0; i < _availableAbilities.Length; i++)
            {
                if (_availableAbilities[i]) _abilities[i] = true;
            }
        }

        if (AbilityIsOn(ref _abilities[0], true))
        {
            // harpoon gun

            _availableAbilities[0] = false;
            _currentAbilityIndex = -1;
        }
        if (AbilityIsOn(ref _abilities[1], true))    
        {
            // caribbean rum

            _remainingMovementSteps += 2;

            _availableAbilities[1] = false;
            _currentAbilityIndex = -1;
        }
        if (AbilityIsOn(ref _abilities[2], false))
        {
            // cursed compass

            if (Input.GetMouseButtonDown(0)) HandlePlayerCLick();
            if (_playerHasBeenSelected)
            {
                _abilities[2] = false;
                int otherPlayerTilePlayernumber = _hitPlayer.GetTilePlayerNumber();
                if (GetTilePlayerNumber() == otherPlayerTilePlayernumber)
                {
                    SwitchPlaces(_hitPlayer);
                    _availableAbilities[2] = false;
                    _currentAbilityIndex = -1;
                }
                _playerHasBeenSelected = false;
            }
        }
        if (AbilityIsOn(ref _abilities[3], true))
        {
            // parrot's warning

            _cantBeKOd = true;
            _cantBeKOdCounter = 0;

            _availableAbilities[3] = false;
            _currentAbilityIndex = -1;
        }
        if (AbilityIsOn(ref _abilities[4], true))
        {
            // broken cannon ball

            _brokenCannon = true;

            _availableAbilities[4] = false;
            _currentAbilityIndex = -1;
        }
        if (AbilityIsOn(ref _abilities[5], true))
        {
            // curse of the flying dutchman

            _canGoThroughWall = true;

            _availableAbilities[5] = false;
            _currentAbilityIndex = -1;
        }
        if (AbilityIsOn(ref _abilities[6], true))
        {
            // Fortify
            Vector3 forwardNormalized = transform.forward.normalized / 2;
            Vector3 position = new Vector3(transform.position.x + forwardNormalized.x, 0.5f, transform.position.z + forwardNormalized.z);
            GameObject wall = Instantiate(_wallPrefab, position, Quaternion.identity);

            _availableAbilities[6] = false;
            _currentAbilityIndex = -1;
        }
        if (AbilityIsOn(ref _abilities[7], false))
        {
            // Davy Jones Locker

            if (Input.GetMouseButtonDown(0)) HandlePlayerCLick();
            if (_playerHasBeenSelected)
            {
                if (_hitPlayer.IsKO)
                {
                    _hitPlayer.IsKO = false;
                    Destroy(_hitPlayer._xInstance);
                    _hitPlayer._xInstance = null;
                }

                _abilities[7] = false;
                _availableAbilities[7] = false;
                _playerHasBeenSelected = false;
                _currentAbilityIndex = -1;
            }
        }


        isBeforeMovement = false;
    }

    private void SwitchPlaces(PlayerControl hitPlayer)
    {
        Vector3 currentLocation = transform.position;
        transform.position = hitPlayer.transform.position;
        _targetPosition = hitPlayer.transform.position;
        hitPlayer.transform.position = currentLocation;
        hitPlayer._targetPosition = currentLocation;
    }

    public int GetTilePlayerNumber()
    {
        int result = 1;

        foreach (GameObject pos in _player1Tiles)
        {
            if (pos.transform.position == transform.position)
            {
                result = 1;
                break;
            }
        }

        foreach (GameObject pos in _player2Tiles)
        {
            if (pos.transform.position == transform.position)
            {
                result = 2;
                break;
            }
        }

        return result;
    }

    private void HandlePlayerCLick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _tileLayer))
        {
            Vector3 _hitPosition = hit.transform.position;
            foreach (PlayerControl control in _players)
            {
                if (control.transform.position == _hitPosition)
                {
                    _hitPlayer = control;
                    _playerHasBeenSelected = true;
                }
            }
        }
    }

    private bool AbilityIsOn(ref bool currentAbility, bool TurnOff)
    {
        if (currentAbility)
        {
            if (TurnOff) currentAbility = !currentAbility;
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
        _cantBeKOdCounter++;
        _remainingMovementSteps = 4;
        _canGoThroughWall = false;
        _isPlayerTurn = false;
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
        if (other.CompareTag("BarrelP1") || other.CompareTag("BarrelP2"))
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
