using System;
using UnityEngine;

public class Boostrap : MonoBehaviour
{
    [SerializeField] private float _spawnDelayInSeconds = 0.1f;
    [SerializeField] private Cube _prefab;
    [SerializeField] private Bomb _prefabBomb;
    [Space]
    [SerializeField] private CountView _countViewCube;
    [SerializeField] private CountView _countViewBomb;

    private Spawner<Cube> _spawnerCubes;
    private Spawner<Bomb> _spawnerBombs;

    private void Awake()
    {
        _spawnerBombs = new Spawner<Bomb>(_prefabBomb);
        _spawnerCubes = new Spawner<Cube>(_prefab, _spawnDelayInSeconds, _spawnerBombs.Spawn);
        
        _countViewBomb.Init("Bomb");
        _countViewCube.Init("Cube");

        _spawnerCubes.CountItemSpawned += _countViewCube.SpawnItem;
        _spawnerCubes.CountItemCreated += _countViewCube.CreatedItem;
        _spawnerCubes.CountItemDespawned += _countViewCube.DespawnItem;
        
        _spawnerBombs.CountItemSpawned += _countViewBomb.SpawnItem;
        _spawnerBombs.CountItemCreated += _countViewBomb.CreatedItem;
        _spawnerBombs.CountItemDespawned += _countViewBomb.DespawnItem;
        
        StartCoroutine(_spawnerCubes.SpawnObjects());

        _countViewCube.SetCounters();
        _countViewBomb.SetCounters();
    }

    private void OnDestroy()
    {        
        _spawnerCubes.CountItemSpawned -= _countViewCube.SpawnItem;
        _spawnerCubes.CountItemCreated -= _countViewCube.CreatedItem;
        _spawnerCubes.CountItemDespawned -= _countViewCube.DespawnItem;
        
        _spawnerBombs.CountItemSpawned -= _countViewBomb.SpawnItem;
        _spawnerBombs.CountItemCreated -= _countViewBomb.CreatedItem;
        _spawnerBombs.CountItemDespawned -= _countViewBomb.DespawnItem;
    }
}