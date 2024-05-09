using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _barrelPrefab;
    [SerializeField]
    private GridGenerator _gridGenerator;

    [Range(0, 10)]
    [SerializeField]
    private int _numberOfBarrels = 1;

    private GameObject _allBarrelsParent;

    void Start()
    {
        _gridGenerator = GameObject.Find("GridGenerator").GetComponent<GridGenerator>();
        _allBarrelsParent = new GameObject("Barrels");
        SpawnBarrelsP1();
        SpawnBarrelsP2();
    }

    private void SpawnBarrelsP1()
    {
        for (int i = 0; i < _numberOfBarrels; i++)
        {
            Vector3 randomPosition = GetRandomGridPositionP1();

            GameObject barrel = Instantiate(_barrelPrefab, randomPosition, _barrelPrefab.transform.rotation, _allBarrelsParent.transform);

            barrel.name = "BarrelP1";
        }
    }

    private void SpawnBarrelsP2()
    {
        for (int i = 0; i < _numberOfBarrels; i++)
        {
            Vector3 randomPosition = GetRandomGridPositionP2();

            GameObject barrel = Instantiate(_barrelPrefab, randomPosition, _barrelPrefab.transform.rotation, _allBarrelsParent.transform);

            barrel.name = "BarrelP2";
        }
    }

    private Vector3 GetRandomGridPositionP1()
    {
        int randomIndex = Random.Range(0, _gridGenerator.TilePositionsP1.Count);
        Vector3 randomPosition = _gridGenerator.TilePositionsP1[randomIndex];

        return randomPosition;
    }

    private Vector3 GetRandomGridPositionP2()
    {
        int randomIndex = Random.Range(0, _gridGenerator.TilePositionsP2.Count);
        Vector3 randomPosition = _gridGenerator.TilePositionsP2[randomIndex];

        return randomPosition;
    }
}
