using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly Func<MonoBehaviour> _create;
    private readonly List<MonoBehaviour> _items;
    private Action<MonoBehaviour> _returnToPool;
    private Counter _counter;
        
    public Pool(MonoBehaviour prefab)
    {
        _items = new List<MonoBehaviour>();
        _counter = new Counter();
        _create = () => Create(prefab);
    }

    private MonoBehaviour Create(MonoBehaviour prefab)
    {
        MonoBehaviour item = MonoBehaviour.Instantiate(prefab);
        _items.Add(item);
        _counter.ObjectsCreated++;

        if (item.gameObject.TryGetComponent<ICounted>(out var component))
            component.DecreaseCount += () => _counter.ObjectsActive--;

        return item;
    }
    
    public MonoBehaviour Get
    {
        get
        {
            var find = _items.Find(item => item.gameObject.activeSelf == false);
            _counter.ObjectsSpawned++;
            _counter.ObjectsActive++;
            return find ?? _create.Invoke();
        }
    }

    public (int created, int spawned, int active) GetCounts =>
        (_counter.ObjectsCreated, _counter.ObjectsSpawned, _counter.ObjectsActive);
}