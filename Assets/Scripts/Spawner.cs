using System;
using System.Collections;
using UnityEngine;

public class Spawner<T> where T : MonoBehaviour, ISpawnable, ICounted
{
    private readonly float _spawnDelayInSeconds;
    private readonly Pool _items;
    private readonly Action<Vector3> _itemSpawned;

    public Action CountItemSpawned;
    public Action CountItemCreated;
    public Action CountItemDespawned;

    public Spawner(T prefab, float spawnDelayInSeconds = 0.1f, Action<Vector3> itemSpawned = null)
    {
        _items = new Pool(prefab);
        _spawnDelayInSeconds = spawnDelayInSeconds;
        _itemSpawned = itemSpawned;
    }

    public IEnumerator SpawnObjects()
    {
        WaitForSeconds wait = new WaitForSeconds(_spawnDelayInSeconds);

        while (true)
        {
            Spawn();
            yield return wait;
        }
    }

    public void Spawn(Vector3 position = default)
    {
        (T item, bool isCreated) result = ((T, bool))_items.Get();
        
        result.item.Spawn(position);
        result.item.Released = _itemSpawned;
        result.item.DecreaseCount = CountItemDespawned;

        CountItemSpawned.Invoke();

        if (result.isCreated)
            CountItemCreated.Invoke();
    }
}