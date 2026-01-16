using System;
using System.Text;
using TMPro;
using UnityEngine;

public class CountView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _counterVisual;
    private string _name;
    private Counter _counter;

    public void Init(string nameView)
    {
        _name = nameView;
        _counter = new Counter();
    }

    public void CreatedItem()
    {
        _counter.ObjectsCreated++;
        SetCounters();
    }
    
    public void DespawnItem()
    {
        _counter.ObjectsActive--;
        SetCounters();
    }
    
    public void SpawnItem()
    {
        _counter.ObjectsActive++;
        _counter.ObjectsSpawned++;
        SetCounters();
    }
    
    public void SetCounters()
    {
        StringBuilder textDescription = new();
        
        textDescription.Append($"{_name}\n" +
                               $"Created: {_counter.ObjectsCreated}\n" +
                               $"Spawned: {_counter.ObjectsSpawned}\n" +
                               $"Active: {_counter.ObjectsActive}\n");
        _counterVisual.text = textDescription.ToString();
    }
}