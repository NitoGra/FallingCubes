using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cube : MonoBehaviour, ICube, ICounted
{
    private const int MinDisappearRangeInSeconds = 2;
    private const int MaxDisappearRangeInSeconds = 5;
    
    [SerializeField] private Renderer _renderer;
    private bool _isDisappeared = false;
    private Func<MonoBehaviour> _spawnBomb;
    
    public Action DecreaseCount { get; set; }
    
    private void OnDisable() => DecreaseCount?.Invoke();


    public void Inst(Func<MonoBehaviour> bombsGet)
    {
        _renderer.material.color = Color.white;
        _spawnBomb = bombsGet;
    }
    
    public void CollideToFloor()
    {
        if (_isDisappeared)
            return;

        _isDisappeared = true;
        ChangeColor();
        Invoke(nameof(Disappear), Random.Range(MinDisappearRangeInSeconds, MaxDisappearRangeInSeconds));
    }

    private void Disappear()
    {
        SpawnBomb();
        gameObject.SetActive(false);
        _isDisappeared = false;
    }

    private void SpawnBomb()
    {
        MonoBehaviour bomb = _spawnBomb.Invoke();
        bomb.transform.position = transform.position;
        bomb.gameObject.SetActive(true);
    }

    private void ChangeColor()
    {
        _renderer.material.color = Random.ColorHSV();
    }
}