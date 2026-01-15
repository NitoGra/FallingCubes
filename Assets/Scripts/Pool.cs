using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private readonly Func<MonoBehaviour> _create;
    private readonly List<MonoBehaviour> _items;
    private Action<MonoBehaviour> _returnToPool;
        
    public Pool(MonoBehaviour prefab)
    {
        _items = new List<MonoBehaviour>();
        _create = () => Create(prefab);
    }

    private MonoBehaviour Create(MonoBehaviour prefab)
    {
        MonoBehaviour item = MonoBehaviour.Instantiate(prefab);
        _items.Add(item);
        return item;
    }

    public MonoBehaviour Get => _items.Find(item => item.gameObject.activeSelf == false) ?? _create.Invoke();
}