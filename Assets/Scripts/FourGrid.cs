using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourGrid : MonoBehaviour
{
    [SerializeField]
    public int FloorWidth;
    [SerializeField]
    public int FloorHeight;
    [SerializeField]
    private int _playersizeX, _playersizeZ, _islandX, _islandZ;

    [SerializeField]
    private GameObject _tilePrefab;
    private GameObject _allTilesParent;

    private Vector3 _floorPosition;
    
    private void Start()
    {
        _allTilesParent = new GameObject("Tiles");
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        _floorPosition = new Vector3(-FloorWidth / 2, 0, -FloorHeight / 2);
        for (int x = 0; x < FloorWidth; x++)
        {
            for (int z = 0; z < FloorHeight; z++)
            {
                if (_floorPosition.x + x <= -_playersizeX && _floorPosition.z + z <= -_playersizeZ)
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "tileRed";
                    tile.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (_floorPosition.x + x <= -_playersizeX && _floorPosition.z + z >= _playersizeZ)
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "tileCyan";
                    tile.GetComponent<Renderer>().material.color = Color.cyan;
                }
                else if (_floorPosition.x + x >= _playersizeX && _floorPosition.z + z >= _playersizeZ)
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "tileMagenta";
                    tile.GetComponent<Renderer>().material.color = Color.magenta;
                }
                else if (_floorPosition.x + x >= _playersizeX && _floorPosition.z + z <= -_playersizeZ)
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "tileBlue";
                    tile.GetComponent<Renderer>().material.color = Color.blue;
                }
                else if (_floorPosition.x + x <= 10 &&
                    _floorPosition.x + x >= -10 &&
                    _floorPosition.z + z >= -1 &&
                    _floorPosition.z + z <= 1)
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "tile";
                }
                else if (_floorPosition.x + x <= 1 &&
                    _floorPosition.x + x >= -1 &&
                    _floorPosition.z + z >= -10 &&
                    _floorPosition.z + z <= 10)
                {
                    Vector3 tilePosition = new Vector3(_floorPosition.x + x, 0, _floorPosition.z + z);
                    GameObject tile = Instantiate(_tilePrefab, tilePosition, _tilePrefab.transform.rotation, _allTilesParent.transform);
                    tile.name = "tile";
                }
            }
        }
    }
}
