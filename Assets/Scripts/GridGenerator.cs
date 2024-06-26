using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [SerializeField]
    public int FloorWidth;
    [SerializeField]
    public int FloorHeight;
    [SerializeField]
    private GameObject _tilePrefab;
    private Vector3 _floorPosition;

    [SerializeField]
    private GameObject _barrelSpawnerPrefab;
    private GameObject _allTilesParent;
    private List<GameObject> _tilePositionsP1 = new List<GameObject>();
    private List<GameObject> _tilePositionsP2 = new List<GameObject>();

    public List<GameObject> TilePositionsP1 { get { return _tilePositionsP1; } }
    public List<GameObject> TilePositionsP2 { get { return _tilePositionsP2; } }

    private List<GameObject> _bridgeTiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        _allTilesParent = new GameObject("Tiles");
        GenerateFloor();
        InstantiateBarrelSpawner();

    }


    // Update is called once per frame
    void Update()
    {

    }
    public bool GenerateBridge(Vector3 position, string name, int direction)
    {
        bool stopPlacingBridge = false;
        for (int x = 1; x < FloorWidth / 3+1; x++)
        {
            for (int z = 0; z < 1; z++)
            {
                Vector3 tilePosition = new Vector3(position.x +x*direction, 0, position.z + z);
                foreach (GameObject bridgeTile in _bridgeTiles)
                {
                    if (tilePosition == bridgeTile.transform.position) stopPlacingBridge = true;
                }
                if (stopPlacingBridge) break;
                GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation);
                tile.name = "bridge";
                tile.GetComponent<Renderer>().material.color = Color.magenta;
                _bridgeTiles.Add(tile);
            }
            if (stopPlacingBridge) break;
        }
        return stopPlacingBridge;
    }
    private void GenerateFloor()
    {
        _floorPosition = new Vector3(-FloorWidth / 2, 0, -FloorHeight / 2);
        for (int x = -2; x < 0; x++)
        {
            for (int z = 0; z < FloorHeight; z++)
            {
                if ((z == MathF.Floor(FloorHeight / 2) || z == MathF.Ceiling(FloorHeight / 2)) && x == -2)
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "player1PickupTile";
                    tile.GetComponent<Renderer>().material.color = Color.yellow;
                    _tilePositionsP1.Add(tile);
                }
                else
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "player1tile";
                    tile.GetComponent<Renderer>().material.color = Color.green;
                    _tilePositionsP1.Add(tile);
                }
            }
        }
        for (int x = 0; x < FloorWidth/3; x++)
        {
            for (int z = 0; z < FloorHeight; z++)
            {
                Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                GameObject tile=Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                tile.name = "tile";
                _tilePositionsP1.Add(tile);

            }
        }
        for (int x = FloorWidth/3*2; x < FloorWidth; x++)
        {
            for (int z = 0; z < FloorHeight; z++)
            {
                Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                GameObject tile=Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                tile.name = "tile";
                _tilePositionsP2.Add(tile);

            }
        }
        for (int x = FloorWidth; x < FloorWidth +2; x++)
        {
            for (int z = 0; z < FloorHeight; z++)
            {
                if ((z == MathF.Floor(FloorHeight / 2) || z == MathF.Ceiling(FloorHeight / 2)) && x == FloorWidth + 1)
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "player2PickupTile";
                    tile.GetComponent<Renderer>().material.color = Color.yellow;
                    _tilePositionsP2.Add(tile);
                }
                else
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "player2tile";
                    tile.GetComponent<Renderer>().material.color = Color.red;
                    _tilePositionsP2.Add(tile);
                }
            }
        }
    }
    private void InstantiateBarrelSpawner()
    {
        GameObject barrelSpawner = Instantiate(_barrelSpawnerPrefab, Vector3.zero, Quaternion.identity);
        barrelSpawner.name = "BarrelSpawner";
    }
}
