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
    private List<Vector3> _tilePositionsP1 = new List<Vector3>();
    private List<Vector3> _tilePositionsP2 = new List<Vector3>();

    public List<Vector3> TilePositionsP1 { get { return _tilePositionsP1; } }
    public List<Vector3> TilePositionsP2 { get { return _tilePositionsP2; } }
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
    public void GenerateBridge(Vector3 position, string name)
    {
        int direction = 0;
        if (String.Compare(name, "pirate1") == 0 ||
            String.Compare(name, "pirate2") == 0 ||
            String.Compare(name, "pirate3") == 0) direction = 1;
        if (String.Compare(name, "pirate4") == 0 ||
            String.Compare(name, "pirate5") == 0 ||
            String.Compare(name, "pirate6") == 0) direction = -1;

        for (int x = 1; x < FloorWidth / 3+1; x++)
        {
            for (int z = 0; z < 1; z++)
            {
                Vector3 tilePosition = new Vector3(position.x +x*direction, 0, position.z + z);
                GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation);
                tile.name = "bridge";
                tile.GetComponent<Renderer>().material.color = Color.magenta;

            }
        }
    }
    private void GenerateFloor()
    {
        _floorPosition = new Vector3(-FloorWidth / 2, 0, -FloorHeight / 2);
        for (int x = -2; x < 0; x++)
        {
            for (int z = 0; z < FloorHeight; z++)
            {
                Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                GameObject tile=Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation,_allTilesParent.transform);
                tile.name = "player1tile";
                tile.GetComponent<Renderer>().material.color = Color.green;
            }
        }
        for (int x = 0; x < FloorWidth/3; x++)
        {
            for (int z = 0; z < FloorHeight; z++)
            {
                Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                GameObject tile=Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                tile.name = "tile";
                _tilePositionsP1.Add(tilePosition);

            }
        }
        for (int x = FloorWidth/3*2; x < FloorWidth; x++)
        {
            for (int z = 0; z < FloorHeight; z++)
            {
                Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                GameObject tile=Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                tile.name = "tile";
                _tilePositionsP2.Add(tilePosition);

            }
        }
        for (int x = FloorWidth; x < FloorWidth +2; x++)
        {
            for (int z = 0; z < FloorHeight; z++)
            {
                Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                tile.name = "player2tile";
                tile.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
    private void InstantiateBarrelSpawner()
    {
        GameObject barrelSpawner = Instantiate(_barrelSpawnerPrefab, Vector3.zero, Quaternion.identity);
        barrelSpawner.name = "BarrelSpawner";
    }
}
